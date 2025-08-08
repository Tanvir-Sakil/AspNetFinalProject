using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.Customers.Queries
{
    public interface IGetCustomersQuery : IDataTables
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public decimal? BalanceFrom { get; set; }

        public decimal? BalanceTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
