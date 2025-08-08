using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Tests.Sales
{
    [ExcludeFromCodeCoverage]
    public class DeleteSaleCommandHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ISalesRepository> _mockSaleRepository;
        private DeleteSaleCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _mockSaleRepository = new Mock<ISalesRepository>();
            _mockUnitOfWork.Setup(u => u.SalesRepository).Returns(_mockSaleRepository.Object);

            _handler = new DeleteSaleCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_SaleExists_ShouldRemoveAndReturnTrue()
        {
            var saleId = Guid.NewGuid();
            var sale = new Sale { Id = saleId };

            _mockSaleRepository.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var command = new DeleteSaleCommand { Id = saleId };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.ShouldBeTrue();
            _mockSaleRepository.Verify(r => r.Remove(sale), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_SaleNotFound_ShouldReturnFalse()
        {
            var saleId = Guid.NewGuid();
            _mockSaleRepository.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

            var command = new DeleteSaleCommand { Id = saleId };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.ShouldBeFalse();
            _mockSaleRepository.Verify(r => r.Remove(It.IsAny<Sale>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
    }
}
