using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{
    public class GetAllUserRolesQueryHandler : IRequestHandler<GetAllUserRolesQuery, List<RoleDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAllUserRolesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<RoleDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await _applicationUnitOfWork.UserRoleRepository.GetAllRolesAsync();
            return userRoles.ToList();
        }
    }
}
