using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Handlers
{
    public class AddBankAccountCommandHandler : IRequestHandler<AddBankAccountCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddBankAccountCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) => _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(AddBankAccountCommand request, CancellationToken cancellationToken)
        {
            var bankAccount = new Domain.Entities.BankAccount
            {
                Id = Guid.NewGuid(),
                AccountName = request.AccountName,
                AccountNo = request.AccountNo,
                BankName = request.BankName,
                BranchName = request.BranchName,
                OpeningBalance = request.OpeningBalance,
                CurrentBalance = request.OpeningBalance,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.BankAccountRepository.AddAsync(bankAccount);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
