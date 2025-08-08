using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Handlers
{
    public class BankAccountUpdateCommandHandler : IRequestHandler<BankAccountUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public BankAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(BankAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var bankAccount = await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(request.Id);
            if (bankAccount == null)
                return false;

            bankAccount.AccountName = request.AccountName;
            bankAccount.AccountNo = request.AccountNo;
            bankAccount.BankName = request.BankName;
            bankAccount.BranchName = request.BranchName;
            bankAccount.OpeningBalance = request.Balance;
            bankAccount.CurrentBalance = request.Balance;
            bankAccount.IsActive = request.IsActive;

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
