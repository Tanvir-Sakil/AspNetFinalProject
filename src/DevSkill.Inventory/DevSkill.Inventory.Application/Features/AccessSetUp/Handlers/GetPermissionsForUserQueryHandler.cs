using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.AccessSetUp.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.AccessSetUp.Handlers
{
    public class GetPermissionsForUserQueryHandler : IRequestHandler<GetPermissionsForUserQuery, List<string>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetPermissionsForUserQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<string>> Handle(GetPermissionsForUserQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UserPermissionRepository.GetPermissionsForUserAsync(request.UserId);
        }
    }

}
