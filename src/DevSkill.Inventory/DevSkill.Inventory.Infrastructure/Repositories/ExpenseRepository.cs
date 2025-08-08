using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Expenses.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ExpenseRepository : Repository<Expense, Guid>, IExpenseRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;

        public ExpenseRepository(ApplicationDbContext dbContext)
       : base(dbContext)
        {
            _applicationDbContext = dbContext;
           
        }

        public async Task<(IList<Expense> Data, int Total, int TotalDisplay)> GetPagedExpensesAsync(IGetExpenseListQuery request)
        {


            Expression<Func<Expense, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                filter = filter.AndAlso(c => c.Name.Contains(request.Name));
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

            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                string searchTerm = request.Search.Value.Trim();

                filter = filter.AndAlso(c =>
                    c.Name.Contains(searchTerm) ||
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

        public IList<Expense> SearchByName(string query, int limit = 20)
        {
            return _applicationDbContext.Set<Expense>()
                .Where(c => c.Name.Contains(query))
                .OrderBy(c => c.Name)
                .Take(limit)
                .ToList();
        }

    
    }
}
