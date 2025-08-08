using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.MobileAccounts.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IMobileAccountRepository :IRepository<DevSkill.Inventory.Domain.Entities.MobileAccount, Guid>
    {
        Task<(IList<DevSkill.Inventory.Domain.Entities.MobileAccount> Data, int Total, int TotalDisplay)> GetPagedMobileAccountsAsync(IGetMobileAccountListQuery request);

        IList<MobileAccount> SearchByName(string query, int limit = 20);

        Task<MobileAccount?> GetByNameAsync(string name);
    }
}
