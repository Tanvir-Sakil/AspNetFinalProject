using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class BankAccountDeleteCommandHandler : IRequestHandler<BankAccountDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public BankAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(BankAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var bankAccount = await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(request.Id);
            if (bankAccount == null)
                return false;

            _applicationUnitOfWork.BankAccountRepository.Remove(bankAccount);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
