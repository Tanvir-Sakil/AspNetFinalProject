using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.Products.Handlers;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.Products
{
    [ExcludeFromCodeCoverage]
    public class SearchProductQueryHandlerTests
    {
        private AutoMock _moq;
        private SearchProductQueryHandler _handler;
        private Mock<IProductRepository> _productRepositoryMock;

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = _moq.Mock<IProductRepository>();
            _handler = new SearchProductQueryHandler(_productRepositoryMock.Object);
        }

        [TearDown]
        public void Teardown() => _productRepositoryMock?.Reset();

        [Test]
        public async Task Handle_ShouldReturnEmptyList_WhenPriceTypeIsNull()
        {
            _productRepositoryMock.Setup(x => x.GetPriceTypeBySaleTypeIdAsync(Guid.NewGuid(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null);

            var query = new SearchProductQuery
            {
                Query = "test",
                SaleTypeId = Guid.NewGuid(),
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.ShouldBeEmpty();
        }

        [Test]
        public async Task Handle_ShouldReturnMappedProducts_WhenPriceTypeIsValid()
        {
            var saleTypeId = Guid.NewGuid();

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "P1", Code = "C1", Stock = 10, MRP = 100, WholesalePrice = 80 },
                new Product { Id = Guid.NewGuid(), Name = "P2", Code = "C2", Stock = 20, MRP = 200, WholesalePrice = 150 }
            };

            _productRepositoryMock.Setup(x => x.GetPriceTypeBySaleTypeIdAsync(saleTypeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync("MRP");

            _productRepositoryMock.Setup(x => x.SearchByNameAsync("test",20))
                .ReturnsAsync(products);

            var query = new SearchProductQuery
            {
                Query = "test",
                SaleTypeId = saleTypeId
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Count.ShouldBe(2);
            result[0].UnitPrice.ShouldBe(products[0].MRP);
            result[1].UnitPrice.ShouldBe(products[1].MRP);
        }
    }
}

