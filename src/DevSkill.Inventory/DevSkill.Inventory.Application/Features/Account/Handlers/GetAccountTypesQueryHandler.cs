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

    public class GetAccountTypesQueryHandler : IRequestHandler<GetAccountTypesQuery, List<AccountTypeDto>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAccountTypesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AccountTypeDto>> Handle(GetAccountTypesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.AccountTypesRepository.GetAllActiveAsync(cancellationToken);
                
        }
    }
}