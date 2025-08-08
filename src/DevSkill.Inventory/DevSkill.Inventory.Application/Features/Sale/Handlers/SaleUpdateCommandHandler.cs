using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class SaleUpdateCommandHandler : IRequestHandler<SaleUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public SaleUpdateCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SaleUpdateCommand request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SalesRepository.GetByIdAsync(request.Id);
            if (sale == null) return false;

            sale.InvoiceNo = request.InvoiceNo;
            sale.Date = request.Date;
            sale.CustomerID = request.CustomerID;
            sale.SalesTypeId = request.SalesType;
            sale.Terms = request.Terms;
            sale.VAT = request.VAT;
            sale.Discount = request.Discount;
            sale.TotalAmount = request.TotalAmount;
            sale.PaidAmount = request.PaidAmount;
            sale.DueAmount = request.DueAmount;
            sale.PaymentStatus = request.PaymentStatus;

            var paymentItem = new PaymentItem
            {
                Id = request.Id,
                AccountNo = request.AccountNo,
                AccountType = request.AccountType,
                Note = request.Note
            };

            sale.PaymentItems.Add(paymentItem);

            foreach (var item in request.Items)
            {
                sale.Items.Add(new SaleItem
                {
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
