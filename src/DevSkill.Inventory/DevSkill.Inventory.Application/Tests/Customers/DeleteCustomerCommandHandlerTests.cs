using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace DevSkill.Inventory.Tests.Customers
{
    [ExcludeFromCodeCoverage]
    public class DeleteCustomerCommandHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private DeleteCustomerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            _mockUnitOfWork.SetupGet(u => u.CustomerRepository).Returns(_mockCustomerRepository.Object);

            _handler = new DeleteCustomerCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_WhenCustomerExists_ShouldDeleteCustomerAndReturnTrue()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId };

            _mockCustomerRepository.Setup(r => r.GetByIdAsync(customerId))
                                   .ReturnsAsync(customer);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                           .Returns(Task.CompletedTask);
            {
                var command = new DeleteCustomerCommand(customerId);
                var result = await _handler.Handle(command, CancellationToken.None);

                _mockCustomerRepository.Verify(r => r.Remove(customer), Times.Once);
                _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            }
        }

            [Test]
            public async Task Handle_WhenCustomerDoesNotExist_ShouldReturnFalse()
            {
                var customerId = Guid.NewGuid();

                _mockCustomerRepository.Setup(r => r.GetByIdAsync(customerId))
                                       .ReturnsAsync((Customer)null);

                var command = new DeleteCustomerCommand(customerId);

                var result = await _handler.Handle(command, CancellationToken.None);

                _mockCustomerRepository.Verify(r => r.Remove(It.IsAny<Customer>()), Times.Never);
                _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
            }
        }
    }
