using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class SearchDepartmentQuery : IRequest<List<SearchDepartmentDto>>
    {
        public string Query { get; set; } = string.Empty;
    }
}
