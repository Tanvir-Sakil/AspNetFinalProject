using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.Sales
{
    [ExcludeFromCodeCoverage]
    public class GetSaleByIdQueryHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ISalesRepository> _salesRepositoryMock;
        private GetSaleByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _salesRepositoryMock = new Mock<ISalesRepository>();
            _unitOfWorkMock.Setup(u => u.SalesRepository).Returns(_salesRepositoryMock.Object);

            _handler = new GetSaleByIdQueryHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSaleDto_WhenSaleExists()
        {
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                InvoiceNo = "INV-001",
                Date = new DateTime(2025, 7, 20),
                CustomerID = Guid.NewGuid(),
                Customer = new Customer
                {
                    Name = "Tanvir",
                    MobileNumber = "123456789",
                    Address = "123 Dhaka"
                },
                SalesTypeId = Guid.NewGuid(),
                SaleType = new SaleType { PriceName = "WholeSale" },
                VAT = 100,
                Discount = 50,
                TotalAmount = 1000,
                PaidAmount = 700,
                DueAmount = 300,
                PaymentStatus = Domain.Enums.PaymentStatus.PartiallyPaid,
                Terms = "Test Terms",
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        Product = new Product { Name = "Product1" },
                        Quantity = 2,
                        UnitPrice = 200
                    },
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        Product = new Product { Name = "Product2" },
                        Quantity = 1,
                        UnitPrice = 300
                    }
                },
                PaymentItems = new List<PaymentItem>
                {
                    new PaymentItem
                    {
                        Id = Guid.NewGuid(),
                        AccountNo = "AC123",
                        AccountType = "Cash",
                        Note = "Test Note",
                    }
                }
            };

            _salesRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(saleId))
                .ReturnsAsync(sale);

            var query = new GetSaleByIdQuery (saleId);

            var result = await _handler.Handle(query, CancellationToken.None);
            result.ShouldNotBeNull();
            result.Id.ShouldBe(sale.Id);
            result.InvoiceNo.ShouldBe("INV-001");
            result.CustomerName.ShouldBe("Tanvir");
            result.PaymentStatus.ShouldBe("PartiallyPaid");
            result.NetTotal.ShouldBe((2 * 200 + 1 * 300) + 100 - 50);
            result.Items.Count.ShouldBe(2);
            result.Items.ToList()[0].ProductName.ShouldBe("Product1");
            result.Items.ToList()[0].SubTotal.ShouldBe(400);
        }

        [Test]
        public async Task Handle_ShouldReturnNull_WhenSaleDoesNotExist()
        {
            var saleId = Guid.NewGuid();
            _salesRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(saleId)).ReturnsAsync((Sale)null);
            var query = new GetSaleByIdQuery (saleId);
            var result = await _handler.Handle(query, CancellationToken.None);
            result.ShouldBeNull();
        }
    }
}

