using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Handlers
{

    public class GetBankAccountByIdQueryHandler : IRequestHandler<GetBankAccountByIdQuery, Domain.Entities.BankAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetBankAccountByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Domain.Entities.BankAccount> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
