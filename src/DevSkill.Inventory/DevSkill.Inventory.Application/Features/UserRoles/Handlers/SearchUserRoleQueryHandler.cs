using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{
    public class SearchUserRoleQueryHandler : IRequestHandler<SearchUserRoleQuery, List<SearchUserRoleDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private readonly IUserRoleRepository _userRoleRepository;

        public SearchUserRoleQueryHandler(IApplicationUnitOfWork userRoleOfWork, IUserRoleRepository userRoleRepository)
        {
            _applicationUnitOfWork = userRoleOfWork;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<SearchUserRoleDto>> Handle(SearchUserRoleQuery request, CancellationToken cancellationToken)
        {
            var userRoles = _userRoleRepository.SearchByName(request.Query);

            var result = userRoles
                .Select(c => new SearchUserRoleDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CompanyName = c.CompanyName!=null? c.CompanyName  : "Unkown"
                })
                .ToList();

            return await Task.FromResult(result);
        }
    }
}
