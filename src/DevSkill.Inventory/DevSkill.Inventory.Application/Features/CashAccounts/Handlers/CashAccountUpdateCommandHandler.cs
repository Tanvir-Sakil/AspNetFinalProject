using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Handlers
{
    public class CashAccountUpdateCommandHandler : IRequestHandler<CashAccountUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CashAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(CashAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var cashAccount = await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(request.Id);
            if (cashAccount == null)
                return false;

            cashAccount.AccountName = request.Name;
            cashAccount.Balance = request.Balance;
            cashAccount.CurrentBalance = request.Balance;
            cashAccount.IsActive = request.IsActive;

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
