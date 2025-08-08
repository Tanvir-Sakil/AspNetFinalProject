using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Tests.Customers
{ 
    [ExcludeFromCodeCoverage]
    public class GetCustomerByIdQueryHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private GetCustomerByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _unitOfWorkMock.Setup(x => x.CustomerRepository).Returns(_customerRepositoryMock.Object);

            _handler = new GetCustomerByIdQueryHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                Name = "Tanvir Sakil",
                CompanyName = "Tech Ltd.",
                MobileNumber = "01700000000",
                Email = "tanvir@gmail.com",
                Address = "Dhaka",
                OpeningBalance = 100000,
                IsActive = true,
                ImagePath = "/images/customer.png"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(customer);

            var query = new GetCustomerByIdQuery { Id = customerId };
            var result = await _handler.Handle(query, CancellationToken.None);
            result.ShouldNotBeNull();
            result.Id.ShouldBe(customer.Id);
            result.Name.ShouldBe(customer.Name);
            result.CompanyName.ShouldBe(customer.CompanyName);
            result.Email.ShouldBe(customer.Email);
        }

        [Test]
        public async Task Handle_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .ReturnsAsync((Customer)null); 

            var query = new GetCustomerByIdQuery { Id = nonExistentId };
            var result = await _handler.Handle(query, CancellationToken.None);

            result.ShouldBeNull();
        }
    }
}

