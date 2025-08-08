using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class BankAccountRepository : Repository<BankAccount, Guid>, IBankAccountRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;

        public BankAccountRepository(ApplicationDbContext dbContext)
       : base(dbContext)
        {
            _applicationDbContext = dbContext;
           
        }

        public async Task<(IList<BankAccount> Data, int Total, int TotalDisplay)> GetPagedBankAccountsAsync(IGetBankAccountListQuery request)
        {


            Expression<Func<BankAccount, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.AccountName))
            {
                filter = filter.AndAlso(c => c.AccountName.Contains(request.AccountName));
            }

            if (request.IsActive.HasValue)
            {
                filter = filter.AndAlso(c => c.IsActive == request.IsActive.Value);
            }
            if (request.CreateDateFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.CreatedDate >= request.CreateDateFrom.Value.Date);
            }

            if (request.CreateDateTo.HasValue)
            {
                filter = filter.AndAlso(c => c.CreatedDate <= request.CreateDateTo.Value.Date);
            }

            if (request.BalanceFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.CurrentBalance <= request.BalanceFrom.Value);
            }
            if (request.BalanceTo.HasValue)
            {
                filter = filter.AndAlso(c => c.CurrentBalance <= request.BalanceTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                string searchTerm = request.Search.Value.Trim();

                filter = filter.AndAlso(c =>
                    c.AccountName.Contains(searchTerm) ||
                    c.IsActive.ToString().Contains(searchTerm) ||
                    c.CreatedDate.ToString().Contains(searchTerm)
                );
            }
            return await GetDynamicAsync(
                filter,
                request.OrderBy,
                null,
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true
            );
        }
        public IList<BankAccount> SearchByName(string query, int limit = 20)
        {
            return _applicationDbContext.Set<BankAccount>()
                .Where(c => c.AccountName.Contains(query))
                .OrderBy(c => c.AccountName)
                .Take(limit)
                .ToList();
        }

        public async Task<BankAccount?> GetByNameAsync(string name)
        {
            return await _applicationDbContext.BankAccounts
                .FirstOrDefaultAsync(u => u.AccountName.ToLower() == name.ToLower());
        }
    }
}
