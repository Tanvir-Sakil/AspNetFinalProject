using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class SearchProductQuery : IRequest<List<ProductDropDownDto>>
    {
        public string Query { get; set; } = string.Empty;
        public Guid SaleTypeId { get; set; }
    }
}
