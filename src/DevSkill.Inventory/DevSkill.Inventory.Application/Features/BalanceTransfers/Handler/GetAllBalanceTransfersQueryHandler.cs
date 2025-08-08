using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Handler
{
    public class GetAllBalanceTransfersQueryHandler : IRequestHandler<GetAllBalanceTransferQuery, (IList<BalanceTransferDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAllBalanceTransfersQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<BalanceTransferDto>, int, int)> Handle(GetAllBalanceTransferQuery request, CancellationToken cancellationToken)
        {
            var (entities,total,totalDisplay) = await _unitOfWork.BalanceTransferRepository.GetAllBalanceTransferAsync(request); 

            var result = new List<BalanceTransferDto>();

            foreach (var transfer in entities)
            {
                dynamic fromRepo = GetRepository(transfer.FromAccountType);
                dynamic toRepo = GetRepository(transfer.ToAccountType);

                dynamic fromAccount = await fromRepo.GetByIdAsync(transfer.FromAccountId);
                dynamic toAccount = await toRepo.GetByIdAsync(transfer.ToAccountId);

                result.Add(new BalanceTransferDto
                {
                    Id = transfer.Id,
                    CreatedAt = transfer.TransferDate,
                    FromAccountName = fromAccount?.AccountName,
                    ToAccountName = toAccount?.AccountName,
                    Amount = transfer.Amount,
                    Note = transfer.Note
                });
            }

            return (result,total,totalDisplay);
        }

        private dynamic GetRepository(string type)
        {
            return type switch
            {
                "Cash" => _unitOfWork.CashAccountRepository,
                "Bank" => _unitOfWork.BankAccountRepository,
                "Mobile" => _unitOfWork.MobileAccountRepository,
                _ => throw new Exception($"Unknown account type: {type}")
            };
        }
    }
}
