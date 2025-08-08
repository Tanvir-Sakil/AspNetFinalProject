using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http.Features;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class PaymentAddCommandHandler : IRequestHandler<PaymentAddCommand,bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public PaymentAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(PaymentAddCommand request, CancellationToken cancellationToken)
        {
            var sale = await _applicationUnitOfWork.SalesRepository.GetByIdAsync(request.SaleID);

            if (sale != null)
            {
                sale.DueAmount -= request.PaidAmount;
                sale.PaidAmount += request.PaidAmount;

                var paymentItem = new PaymentItem
                {
                    Id = Guid.NewGuid(),
                    AccountNo = request.AccountNo,
                    AccountType = request.AccountType
                };
                sale.PaymentItems.Add(paymentItem);

                if (sale.DueAmount <= 0)
                {
                    sale.PaymentStatus = PaymentStatus.FullyPaid;
                }
                else if (sale.DueAmount > 0 && sale.PaidAmount > 0)
                {
                    sale.PaymentStatus = PaymentStatus.PartiallyPaid;
                }
                else
                {
                    sale.PaymentStatus = PaymentStatus.Due;
                }
                _applicationUnitOfWork.SalesRepository.Update(sale);
                await _applicationUnitOfWork.SaveChangesAsync();
            }
            
            return true;
        }
    }
}
