using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class GetCustomersQuery : DataTables, IRequest<(IList<Customer>, int, int)>, IGetCustomersQuery
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public decimal? BalanceFrom { get; set; }

        public decimal? BalanceTo { get; set; }

        public string? OrderBy { get; set; }

    }
}
