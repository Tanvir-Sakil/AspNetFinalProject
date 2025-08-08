using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Account.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Account.Handlers
{
    public class GetAccountsByTypeNameQueryHandler : IRequestHandler<GetAccountsByTypeNameQuery, List<AccountTypeDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAccountsByTypeNameQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<AccountTypeDto>> Handle(GetAccountsByTypeNameQuery request, CancellationToken cancellationToken)
        {
            
            return await _applicationUnitOfWork.AccountSearchRepository.GetAccountsByTypeNameAsync(request.AccountTypeName, cancellationToken);
        }
    }

}
