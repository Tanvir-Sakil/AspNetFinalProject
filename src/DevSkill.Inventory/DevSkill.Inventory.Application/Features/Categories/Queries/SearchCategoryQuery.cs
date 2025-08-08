using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class SearchCategoryQuery : IRequest<List<SearchCategoryDto>>
    {
        public string Query { get; set; } = string.Empty;
    }
}
