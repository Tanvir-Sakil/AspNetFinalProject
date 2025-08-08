using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Tests.Sales
{
    [ExcludeFromCodeCoverage]
    public class GetAllSalesQueryHandlerTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ISalesRepository> _mockSalesRepository;
        private GetAllSalesQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _mockSalesRepository = new Mock<ISalesRepository>();
            _mockUnitOfWork.Setup(u => u.SalesRepository).Returns(_mockSalesRepository.Object);

            _handler = new GetAllSalesQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnMappedSalesDto_WithCorrectCounts()
        {
            var query = new GetAllSalesQuery(); 

            var sales = new List<Sale>
            {
                new Sale
                {
                    Id = Guid.NewGuid(),
                    InvoiceNo = "INV-001",
                    Date = DateTime.Today,
                    Customer = new Domain.Entities.Customer { Name = "Tanvir" },
                    TotalAmount = 1500,
                    PaidAmount = 1500,
                    DueAmount = 0,
                    PaymentStatus = PaymentStatus.FullyPaid
                },
                new Sale
                {
                    Id = Guid.NewGuid(),
                    InvoiceNo = "INV-002",
                    Date = DateTime.Today.AddDays(-1),
                    Customer = new Domain.Entities.Customer { Name = "Sakil" },
                    TotalAmount = 2000,
                    PaidAmount = 100,
                    DueAmount = 1000,
                    PaymentStatus = PaymentStatus.PartiallyPaid
                }
            };

            int totalCount = 10;
            int totalDisplayCount = 2;

            _mockSalesRepository
                .Setup(r => r.GetAllSaleAsync(query))
                .ReturnsAsync((sales, totalCount, totalDisplayCount));
            var (result, total, totalDisplay) = await _handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            total.ShouldBe(totalCount);
            totalDisplay.ShouldBe(totalDisplayCount);

            var firstSale = result[0];
            firstSale.InvoiceNo.ShouldBe("INV-001");
            firstSale.CustomerName.ShouldBe("Tanvir");
            firstSale.PaymentStatus.ShouldBe("Full Paid");

            var secondSale = result[1];
            secondSale.InvoiceNo.ShouldBe("INV-002");
            secondSale.CustomerName.ShouldBe("Sakil");
            secondSale.PaymentStatus.ShouldBe("Partial Paid");


            _mockSalesRepository.Verify(r => r.GetAllSaleAsync(query), Times.Once);
        }
    }
}

