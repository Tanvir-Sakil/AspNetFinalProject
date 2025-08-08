using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using System.Collections.Immutable;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class GetSaleByInvoiceQueryHandler : IRequestHandler<GetSaleByInvoiceQuery, SaleDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetSaleByInvoiceQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SaleDto> Handle(GetSaleByInvoiceQuery request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SalesRepository.GetByInvoiceWithDetailsAsync(request.Invoice);
            if (sale == null) return null;

            return new SaleDto
            {
                Id = sale.Id,
                InvoiceNo = sale.InvoiceNo,
                Date = sale.Date,
                CustomerID = sale.CustomerID,
                CustomerName = sale.Customer?.Name,
                CustomerContact = sale.Customer?.MobileNumber,
                CustomerAddress = sale.Customer?.Address,
                SalesType = sale.SalesTypeId,
                SaleTypeName  = sale.SaleType.PriceName,
                VAT = sale.VAT,
                Discount = sale.Discount,
                TotalAmount = sale.TotalAmount,
                PaidAmount = sale.PaidAmount,
                DueAmount = sale.DueAmount,
                PaymentStatus = sale.PaymentStatus.ToString(),
                Terms = sale.Terms,
                PaymentItems = sale.PaymentItems.Select(i => new PaymentItemDto
                {
                     PaymentID = i.Id,
                     AccountNo = i.AccountNo,
                     AccountType = i.AccountType,
                    Note = i.Note,
                }).ToList(),
                Items = sale.Items.Select(i => new SaleItemDto
                {
                    ProductID = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    ReturnedQuantity = i.ReturnedQuantity,
                    ReturnableQty = i.Quantity - i.ReturnedQuantity,
                    UnitPrice = i.UnitPrice,

                }
                ).ToList()
            };
        }
    }
}
