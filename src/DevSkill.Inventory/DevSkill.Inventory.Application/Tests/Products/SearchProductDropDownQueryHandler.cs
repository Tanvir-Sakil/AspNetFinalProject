using System;
using System.Collections.Generic;
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
    public class SearchProductDropdownQueryHandlerTests
    {
        private AutoMock _moq;
        private SearchProductDropdownQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<SearchProductDropdownQueryHandler>();
            _unitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        }

        [TearDown]
        public void Teardown() => _unitOfWorkMock?.Reset();

        [Test]
        public async Task Handle_ShouldReturnSearchDropdownList()
        {
            var expectedList = new List<SearchProductDropdownDto>
            {
                new SearchProductDropdownDto { Id = Guid.NewGuid(), Name = "Test Product" }
            };

            var queryText = "test";

            _unitOfWorkMock.Setup(x => x.ProductRepository.SearchDropdownProductsAsync(queryText, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedList);

            var query = new SearchProductDropdownQuery { Query = queryText };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expectedList);
        }
    }
}

