using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class MobileAccountDeleteCommandHandler : IRequestHandler<MobileAccountDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public MobileAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(MobileAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var mobileAccount = await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(request.Id);
            if (mobileAccount == null)
                return false;

            _applicationUnitOfWork.MobileAccountRepository.Remove(mobileAccount);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
