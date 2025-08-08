using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.CashAccounts.Queries
{
    public interface IGetCashAccountListQuery :IDataTables
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDateFrom  { get; set; }


        public DateTime? CreateDateTo { get; set; }

        public decimal ? BalanceFrom { get; set; }

        public decimal ?  BalanceTo {  get; set; } 

        public string? OrderBy { get; set; }
    }
}
