using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.CashAccounts.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICashAccountRepository :IRepository<DevSkill.Inventory.Domain.Entities.CashAccount, Guid>
    {
        Task<(IList<DevSkill.Inventory.Domain.Entities.CashAccount> Data, int Total, int TotalDisplay)> GetPagedCashAccountsAsync(IGetCashAccountListQuery request);

        IList<CashAccount> SearchByName(string query, int limit = 20);

        Task<CashAccount?> GetByNameAsync(string name);
    }
}
