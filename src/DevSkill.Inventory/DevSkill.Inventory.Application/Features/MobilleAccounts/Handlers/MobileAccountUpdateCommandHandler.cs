using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Handlers
{
    public class MobileAccountUpdateCommandHandler : IRequestHandler<MobileAccountUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public MobileAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(MobileAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var mobileAccount = await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(request.Id);
            if (mobileAccount == null)
                return false;

            mobileAccount.AccountName = request.AccountName;
            mobileAccount.AccountNo = request.AccountNo;
            mobileAccount.OwnerName = request.OwnerName;
            mobileAccount.OpeningBalance = request.Balance;
            mobileAccount.CurrentBalance = request.Balance;
            mobileAccount.IsActive = request.IsActive;

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
