using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Tests.Customers
{
    [ExcludeFromCodeCoverage]
    public class UpdateCustomerCommandHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IFileUploader> _fileUploaderMock;
        private UpdateCustomerCommandHandler _handler;
        private Mock<ISqsRepository> _sqsRepositoryMock;
        private Mock<ICustomerRepository> _customerRepoMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _fileUploaderMock = new Mock<IFileUploader>();
            _sqsRepositoryMock = new Mock<ISqsRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock.Setup(x => x.CustomerRepository).Returns(_customerRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.SqsRepository).Returns(_sqsRepositoryMock.Object);
            _handler = new UpdateCustomerCommandHandler(_unitOfWorkMock.Object, _fileUploaderMock.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdateCustomerAndUploadImage()
        {
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer { Id = customerId };

            _customerRepoMock.Setup(x => x.GetByIdAsync(customerId))
                .ReturnsAsync(existingCustomer);

            _fileUploaderMock.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("/uploads/customers/image.png");

            var command = new UpdateCustomerCommand
            {
                Id = customerId,
                Name = "Updated Name",
                MobileNumber = "01978600000",
                Email = "Sakil@gmail.com",
                Address = "Updated Address",
                OpeningBalance = 20000,
                IsActive = true,
                ImageFile = new byte[0],
                ImageFileName = "update.jpg"
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            existingCustomer.Name.ShouldBe("Updated Name");
            _unitOfWorkMock.Verify(x => x.CustomerRepository.Update(existingCustomer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFalse_WhenCustomerNotFound()
        {
            var nonExistentId = Guid.NewGuid();

            _unitOfWorkMock.Setup(x => x.CustomerRepository.GetByIdAsync(nonExistentId))
                .ReturnsAsync((Customer)null);

            var command = new UpdateCustomerCommand { Id = nonExistentId };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.ShouldBeFalse();
        }
    }
}

