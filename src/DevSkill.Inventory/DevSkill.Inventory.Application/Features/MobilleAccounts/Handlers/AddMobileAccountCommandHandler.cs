using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Handlers
{
    public class AddMobileAccountCommandHandler : IRequestHandler<AddMobileAccountCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddMobileAccountCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) => _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(AddMobileAccountCommand request, CancellationToken cancellationToken)
        {
            var mobileAccount = new Domain.Entities.MobileAccount
            {
                Id = Guid.NewGuid(),
                AccountName = request.AccountName,
                AccountNo = request.AccountNo,
                OwnerName = request.OwnerName,
                OpeningBalance = request.OpeningBalance,
                CurrentBalance = request.OpeningBalance,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.MobileAccountRepository.AddAsync(mobileAccount);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
