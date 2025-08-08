using System;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Application.Features.CustomerLedger.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Utilities;
using MediatR;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Tests.Customers
{
    [ExcludeFromCodeCoverage]
    public class CustomerAddCommandHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<IFileUploader> _fileUploaderMock;
        private Mock<ISqsRepository> _sqsRepositoryMock;
        private Mock<ICustomerRepository> _customerRepoMock;
        private CustomerAddCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _fileUploaderMock = new Mock<IFileUploader>();
            _sqsRepositoryMock = new Mock<ISqsRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock.Setup(x => x.CustomerRepository).Returns(_customerRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.SqsRepository).Returns(_sqsRepositoryMock.Object);

            _handler = new CustomerAddCommandHandler(
                _unitOfWorkMock.Object,
                _mediatorMock.Object,
                _fileUploaderMock.Object
            );
        }

        [Test]
        public async Task Handle_ShouldAddCustomerWithImageAndOpeningBalance()
        {
            var customerId = Guid.NewGuid();
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GenerateNextCustomerCodeAsync())
                .ReturnsAsync("CUST001");

            _fileUploaderMock.Setup(f => f.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("/uploads/customers/image.png");

            var command = new CustomerAddCommand
            {
                Name = "Tanvir Sakil",
                CompanyName = "Tech Corp",
                MobileNumber = "01700000000",
                Email = "tanvir@gmail.com",
                Address = "Dhaka",
                OpeningBalance = 1000,
                ImageFileName = "image.jpg",
                ImageFile = new byte[0] 
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.ShouldNotBe(Guid.Empty);
            _unitOfWorkMock.Verify(x => x.CustomerRepository.AddAsync(It.IsAny<Customer>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AddCustomerLedgerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldAddCustomerWithoutOpeningBalance()
        {
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GenerateNextCustomerCodeAsync()).ReturnsAsync("CUST002");

            var command = new CustomerAddCommand
            {
                Name = "No Balance User",
                OpeningBalance = 0
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.ShouldNotBe(Guid.Empty);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AddCustomerLedgerCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

