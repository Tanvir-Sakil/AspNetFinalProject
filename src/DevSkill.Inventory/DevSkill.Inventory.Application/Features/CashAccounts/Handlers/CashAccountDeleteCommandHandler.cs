using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class CashAccountDeleteCommandHandler : IRequestHandler<CashAccountDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CashAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(CashAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var cashAccount = await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(request.Id);
            if (cashAccount == null)
                return false;

            _applicationUnitOfWork.CashAccountRepository.Remove(cashAccount);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
