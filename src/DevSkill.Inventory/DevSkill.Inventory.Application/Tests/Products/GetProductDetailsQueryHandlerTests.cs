using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.Products.Handlers;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.Products
{
    [ExcludeFromCodeCoverage]
    public class GetProductDetailsQueryHandlerTests
    {
        private AutoMock _moq;
        private GetProductDetailsQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        private Guid _productId = Guid.NewGuid();
        private Guid _saleTypeId = Guid.NewGuid();

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<GetProductDetailsQueryHandler>();
            _unitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        }

        [TearDown]
        public void Teardown() => _unitOfWorkMock?.Reset();

        [Test]
        public async Task Handle_ShouldReturnProductDto_WhenFound()
        {
            var productDto = new ProductDto { Id = _productId };

            _unitOfWorkMock.Setup(x => x.ProductRepository.GetProductBySaleTypeAsync(_productId, _saleTypeId))
                .ReturnsAsync(productDto);

            var query = new GetProductDetailsQuery
            {
                ProductId = _productId,
                SaleTypeId = _saleTypeId
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.ShouldBe(productDto);
        }
    }
}

