using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.BankAccounts.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IBankAccountRepository :IRepository<DevSkill.Inventory.Domain.Entities.BankAccount, Guid>
    {
        Task<(IList<DevSkill.Inventory.Domain.Entities.BankAccount> Data, int Total, int TotalDisplay)> GetPagedBankAccountsAsync(IGetBankAccountListQuery request);

        IList<BankAccount> SearchByName(string query, int limit = 20);

        Task<BankAccount?> GetByNameAsync(string name);
    }
}
