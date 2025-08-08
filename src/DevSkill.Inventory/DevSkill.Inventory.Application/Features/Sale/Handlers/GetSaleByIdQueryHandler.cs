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

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetSaleByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SaleDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SalesRepository.GetByIdWithDetailsAsync(request.Id);
            if (sale == null) return null;

            decimal subtotal = sale.Items.Sum(i => (decimal)i.Quantity * i.UnitPrice);

            decimal vatAmount = sale.VAT ;
            decimal discountAmount = sale.Discount ;


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
                NetTotal = subtotal + vatAmount - discountAmount,
                TotalAmount = sale.TotalAmount,
                PaidAmount = sale.PaidAmount,
                DueAmount = sale.DueAmount,
                PaymentStatus = sale.PaymentStatus.ToString(),
                Terms = sale.Terms,
                Items = sale.Items.Select(i => new SaleItemDto
                {
                    ProductID = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    SubTotal = (decimal)i.Quantity*(decimal)i.UnitPrice, 
                }
                ).ToList(),
                PaymentItems = sale.PaymentItems.Select(i => new PaymentItemDto
                {
                    PaymentID = i.Id,
                    AccountNo = i.AccountNo,
                    AccountType = i.AccountType,
                    Note = i.Note,
                }
                ).ToList()
            };
        }
    }
}
