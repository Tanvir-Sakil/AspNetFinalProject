using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Dtos;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Tests.Sales
{
    [ExcludeFromCodeCoverage]
    public class SaleUpdateCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldUpdateSaleAndReturnTrue_WhenSaleExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem>() // Ensure it's initialized
            };

            var mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            var mockSalesRepo = new Mock<ISalesRepository>();
            mockSalesRepo.Setup(repo => repo.GetByIdAsync(saleId))
                         .ReturnsAsync(existingSale);
            mockUnitOfWork.Setup(u => u.SalesRepository).Returns(mockSalesRepo.Object);
            mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var handler = new SaleUpdateCommandHandler(mockUnitOfWork.Object);

            var request = new SaleUpdateCommand
            {
                Id = saleId,
                Date = DateTime.Today,
                CustomerID = Guid.NewGuid(),
                SalesType = Guid.NewGuid(),
                AccountType = "Bank",
                AccountNo = "123456789",
                Note = "Updated note",
                Terms = "30 Days",
                VAT = 7,
                Discount = 5,
                TotalAmount = 1000,
                PaidAmount = 700,
                DueAmount = 300,
                PaymentStatus = PaymentStatus.PartiallyPaid,
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { ProductID = Guid.NewGuid(), Quantity = 2, UnitPrice = 200 },
                    new SaleItemDto { ProductID = Guid.NewGuid(), Quantity = 1, UnitPrice = 600 }
                }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldBeTrue();
            existingSale.TotalAmount.ShouldBe(1000);
            existingSale.Items.Count.ShouldBe(2);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFalse_WhenSaleNotFound()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            var mockSalesRepo = new Mock<ISalesRepository>();

            mockSalesRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((Sale)null);

            mockUnitOfWork.Setup(u => u.SalesRepository).Returns(mockSalesRepo.Object);

            var handler = new SaleUpdateCommandHandler(mockUnitOfWork.Object);

            var request = new SaleUpdateCommand
            {
                Id = Guid.NewGuid()
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
    }
}
