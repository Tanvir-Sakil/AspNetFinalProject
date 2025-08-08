using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Tests.Application.Customers
{
    [ExcludeFromCodeCoverage]
    public class GetCustomerQueryHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private GetCustomerQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _handler = new GetCustomerQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnCustomerListWithCounts()
        {
            var fakeQuery = new GetCustomersQuery(); 
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "Tanvir" },
                new Customer { Id = Guid.NewGuid(), Name = "Sakil" }
            };

            int totalCount = 5;
            int filteredCount = 2;

            _mockUnitOfWork.Setup(x => x.CustomerRepository.GetPagedCustomerAsync(fakeQuery))
                           .ReturnsAsync((customers, totalCount, filteredCount));

            var result = await _handler.Handle(fakeQuery, CancellationToken.None);

            _mockUnitOfWork.Verify(x => x.CustomerRepository.GetPagedCustomerAsync(fakeQuery), Times.Once);
        }
    }
}
