using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Command;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Handler
{
    public class BalanceTransferCommandHandler : IRequestHandler<AddBalanceTransferCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public BalanceTransferCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(AddBalanceTransferCommand request, CancellationToken cancellationToken)
        {
            dynamic fromAccount = await GetRepository(request.FromAccountType).GetByIdAsync(request.FromAccountId);
            dynamic toAccount = await GetRepository(request.ToAccountType).GetByIdAsync(request.ToAccountId);

            if (fromAccount == null || toAccount == null)
                throw new Exception("Invalid accounts"); 

            if (fromAccount.CurrentBalance < request.Amount)
                throw new Exception("Insufficient balance");

            fromAccount.CurrentBalance -= request.Amount;
            toAccount.CurrentBalance += request.Amount;

            var transferRecord = new BalanceTransfer
            {
                FromAccountType = request.FromAccountType,
                FromAccountId = request.FromAccountId,
                ToAccountType = request.ToAccountType,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Note = request.Note,
                TransferDate = DateTime.UtcNow
            };

            await _applicationUnitOfWork.BalanceTransferRepository.AddAsync(transferRecord);

            await _applicationUnitOfWork.SaveAsync();
            return true;
        }

        private dynamic GetRepository(string type)
        {
            return type switch
            {
                "Cash" => _applicationUnitOfWork.CashAccountRepository,
                "Bank" => _applicationUnitOfWork.BankAccountRepository,
                "Mobile" => _applicationUnitOfWork.MobileAccountRepository,
                _ => throw new Exception("Unknown account type")
            };
        }
    }

}
