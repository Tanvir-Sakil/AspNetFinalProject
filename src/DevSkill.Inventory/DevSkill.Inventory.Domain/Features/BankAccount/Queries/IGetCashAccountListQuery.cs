using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.BankAccounts.Queries
{
    public interface IGetBankAccountListQuery :IDataTables
    {
        public string? AccountName { get; set; }

        public string? AccountNo { get; set; }

        public string? BankName { get; set; }

        public string? BranchName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDateFrom  { get; set; }


        public DateTime? CreateDateTo { get; set; }

        public decimal ? BalanceFrom { get; set; }

        public decimal ?  BalanceTo {  get; set; } 

        public string? OrderBy { get; set; }
    }
}
