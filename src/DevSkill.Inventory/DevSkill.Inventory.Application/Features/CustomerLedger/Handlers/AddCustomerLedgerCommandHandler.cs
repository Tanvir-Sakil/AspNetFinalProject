using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CustomerLedger.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CustomerLedger.Handlers
{
        public class AddCustomerLedgerCommandHandler : IRequestHandler<AddCustomerLedgerCommand, Guid>
        {
            private readonly IApplicationUnitOfWork _unitOfWork;

            public AddCustomerLedgerCommandHandler(IApplicationUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Guid> Handle(AddCustomerLedgerCommand request, CancellationToken cancellationToken)
            {
            var lastBalance = await _unitOfWork.CustomerLedgerRepository.GetLastBalanceAsync(request.CustomerId);


            var newBalance = lastBalance + request.Total - request.Paid;

                var ledger = new DevSkill.Inventory.Domain.Entities.CustomerLedger
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    Date = request.Date,
                    InvoiceNo = request.InvoiceNo,
                    Particulars = request.Particulars,
                    Total = request.Total,
                    Discount = request.Discount,
                    Vat = request.Vat,
                    Paid = request.Paid,
                    Balance = newBalance,
                    SourceType = request.SourceType,
                    SourceId = request.SourceId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.CustomerLedgerRepository.AddAsync(ledger);
                await _unitOfWork.SaveAsync();

                return ledger.Id;
            }
        }
}


