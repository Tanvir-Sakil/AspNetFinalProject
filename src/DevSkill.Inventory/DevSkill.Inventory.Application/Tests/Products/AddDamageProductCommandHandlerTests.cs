using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;


namespace DevSkill.Inventory.Application.Tests.Products
{
    [ExcludeFromCodeCoverage]
    public class AddDamageProductCommandHandlerTests
    {
        private AutoMock _moq;
        private AddDamageProductCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        private Guid _productId = Guid.NewGuid();

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<AddDamageProductCommandHandler>();
            _unitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        }

        [TearDown]
        public void Teardown() => _unitOfWorkMock?.Reset();

        [Test]
        public async Task Handle_ShouldUpdateDamageAndStock_WhenProductExists()
        {
            var product = new Product { Id = _productId, Damage = 5, Stock = 10 };

            _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            var command = new AddDamageProductCommand
            {
                ProductId = _productId,
                Quantity = 3
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.ShouldBeTrue();
            product.Damage.ShouldBe(8);
            product.Stock.ShouldBe(7);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            var command = new AddDamageProductCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 3
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.ShouldBeFalse();
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}


