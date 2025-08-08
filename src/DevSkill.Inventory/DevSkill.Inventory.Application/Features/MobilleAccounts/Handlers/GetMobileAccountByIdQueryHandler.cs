using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Handlers
{

    public class GetMobileAccountByIdQueryHandler : IRequestHandler<GetMobileAccountByIdQuery, Domain.Entities.MobileAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetMobileAccountByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Domain.Entities.MobileAccount> Handle(GetMobileAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
