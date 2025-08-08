using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Handlers
{
    public class AddCashAccountCommandHandler : IRequestHandler<AddCashAccountCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddCashAccountCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) => _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(AddCashAccountCommand request, CancellationToken cancellationToken)
        {
            var cashAccount = new Domain.Entities.CashAccount
            {
                Id = Guid.NewGuid(),
                AccountName = request.Name,
                Balance = request.Balance,
                CurrentBalance = request.Balance,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.CashAccountRepository.AddAsync(cashAccount);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
