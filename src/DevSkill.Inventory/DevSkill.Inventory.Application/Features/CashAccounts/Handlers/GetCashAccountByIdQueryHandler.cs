using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Handlers
{

    public class GetCashAccountByIdQueryHandler : IRequestHandler<GetCashAccountByIdQuery, Domain.Entities.CashAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetCashAccountByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Domain.Entities.CashAccount> Handle(GetCashAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
