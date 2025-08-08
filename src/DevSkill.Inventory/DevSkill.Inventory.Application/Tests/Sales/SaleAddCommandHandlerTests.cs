using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;
using Shouldly;
using DevSkill.Inventory.Application.Features.CustomerLedger.Commands;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Tests.Sales
{
    [ExcludeFromCodeCoverage]
    public class SaleAddCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldAddSaleAndSendCustomerLedgerCommand_ReturnsTrue()
        {
            var mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            var mockSalesRepo = new Mock<ISalesRepository>();
            var mockMediator = new Mock<IMediator>();

            mockUnitOfWork.Setup(u => u.SalesRepository).Returns(mockSalesRepo.Object);
            mockSalesRepo.Setup(s => s.GenerateInvoiceAsync()).ReturnsAsync("INV-123");

            var handler = new SaleAddCommandHandler(mockUnitOfWork.Object, mockMediator.Object);

            var request = new SaleAddCommand
            {
                CustomerID = Guid.NewGuid(),
                SalesType = Guid.NewGuid(),
                AccountType = "Cash",
                AccountNo = "AC123",
                Note = "Test Note",
                Terms = "Immediate",
                VAT = 5,
                Discount = 10,
                TotalAmount = 500,
                PaidAmount = 300,
                DueAmount = 200,
                PaymentStatus = PaymentStatus.PartiallyPaid,
                Date = DateTime.Today,
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { ProductID = Guid.NewGuid(), Quantity = 2, UnitPrice = 100 },
                    new SaleItemDto { ProductID = Guid.NewGuid(), Quantity = 1, UnitPrice = 300 }
                }
            };

            Sale savedSale = null;

            mockSalesRepo.Setup(s => s.AddSaleAsync(It.IsAny<Sale>()))
                .Callback<Sale>(s => savedSale = s)
                .Returns(Task.CompletedTask);

            mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            mockMediator.Setup(m => m.Send(It.IsAny<AddCustomerLedgerCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Guid.NewGuid());

            var result = await handler.Handle(request, CancellationToken.None);


            result.ShouldBeTrue();

            mockSalesRepo.Verify(s => s.GenerateInvoiceAsync(), Times.Once);
            mockSalesRepo.Verify(s => s.AddSaleAsync(It.IsAny<Sale>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            mockMediator.Verify(m => m.Send(It.IsAny<AddCustomerLedgerCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            savedSale.ShouldNotBeNull();
            savedSale.InvoiceNo.ShouldBe("INV-123");
            savedSale.Items.Count.ShouldBe(2);
            savedSale.TotalAmount.ShouldBe(500);
        }
    }
}

