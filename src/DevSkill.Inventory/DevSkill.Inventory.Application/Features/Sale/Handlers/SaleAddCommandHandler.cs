using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Application.Features.CustomerLedger.Commands;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
 

    public class SaleAddCommandHandler : IRequestHandler<SaleAddCommand, bool>
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMediator _mediator;

        public SaleAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMediator mediator)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> Handle(SaleAddCommand request, CancellationToken cancellationToken)
        {
            var invoiceNo = await _applicationUnitOfWork.SalesRepository.GenerateInvoiceAsync();

            var sale = new DevSkill.Inventory.Domain.Entities.Sale
            {
                InvoiceNo = invoiceNo,
                Date = request.Date,
                CustomerID = request.CustomerID,
                SalesTypeId = request.SalesType,
                Terms = request.Terms,
                VAT = request.VAT,
                Discount = request.Discount,
                TotalAmount = request.TotalAmount,
                PaidAmount = request.PaidAmount,
                DueAmount = request.DueAmount,
                PaymentStatus = request.PaymentStatus
            };

            foreach (var item in request.Items)
            {
                sale.Items.Add(new SaleItem
                {
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }
            var paymentItem = new PaymentItem
            {
                Id = Guid.NewGuid(),
                AccountNo = request.AccountNo,
                AccountType = request.AccountType,
                Note = request.Note,
            };

            await _applicationUnitOfWork.SalesRepository.AddSaleAsync(sale);
            await _applicationUnitOfWork.SaveAsync();


            await _mediator.Send(new AddCustomerLedgerCommand
            {
                CustomerId = request.CustomerID,
                InvoiceNo = invoiceNo,
                Particulars = "Product Sale",
                Total = request.TotalAmount,
                Discount = (decimal)request.Discount,
                Vat = (decimal)request.VAT,
                Paid = request.PaidAmount,
                Date = request.Date,
                SourceType = "Sale",
                SourceId = sale.Id
            });
            return true;
        }
    }

}
