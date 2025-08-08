using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Features.CashAccounts.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CashAccountRepository : Repository<CashAccount, Guid>, ICashAccountRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;

        public CashAccountRepository(ApplicationDbContext dbContext)
       : base(dbContext)
        {
            _applicationDbContext = dbContext;
           
        }

        public async Task<(IList<DevSkill.Inventory.Domain.Entities.CashAccount> Data, int Total, int TotalDisplay)> GetPagedCashAccountsAsync(IGetCashAccountListQuery request)
        {


            Expression<Func<CashAccount, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                filter = filter.AndAlso(c => c.AccountName.Contains(request.Name));
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
                filter = filter.AndAlso(c => c.Balance <= request.BalanceFrom.Value);
            }
            if (request.BalanceTo.HasValue)
            {
                filter = filter.AndAlso(c => c.Balance <= request.BalanceTo.Value);
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
        public IList<CashAccount> SearchByName(string query, int limit = 20)
        {
            return _applicationDbContext.Set<CashAccount>()
                .Where(c => c.AccountName.Contains(query))
                .OrderBy(c => c.AccountName)
                .Take(limit)
                .ToList();
        }

        public async Task<CashAccount?> GetByNameAsync(string name)
        {
            return await _applicationDbContext.CashAccounts
                .FirstOrDefaultAsync(u => u.AccountName.ToLower() == name.ToLower());
        }
    }
}
