using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class GetProductDetailsQuery : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
        public Guid SaleTypeId { get; set; }
    }

}
