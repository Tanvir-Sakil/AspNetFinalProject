using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Application.Features.Departments.Queries;

namespace DevSkill.Inventory.Application.Features.Departments.Handlers
{
    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartmentQuery, List<SearchDepartmentDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public SearchDepartmentQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<SearchDepartmentDto>> Handle(SearchDepartmentQuery request, CancellationToken cancellationToken)
        {
            var departments = _applicationUnitOfWork.DepartmentRepository.SearchByName(request.Query);

            var result = departments
                .Select(c => new SearchDepartmentDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return await Task.FromResult(result);
        }
    }
}
