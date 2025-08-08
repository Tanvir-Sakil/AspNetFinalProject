using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICustomerLedgerRepository : IRepository<CustomerLedger, Guid>
    {
        Task<List<CustomerLedger>> GetCustomerLedgerAsync(Guid customerId, string reportType,
            DateTime? start, DateTime? end, int? month, int? year, int? reportYear);
        Task<decimal> GetLastBalanceAsync(Guid customerId);
    }
}
