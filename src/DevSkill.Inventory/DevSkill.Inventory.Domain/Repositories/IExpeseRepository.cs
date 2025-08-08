using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Expenses.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IExpenseRepository :IRepository<Expense, Guid>
    {
        Task<(IList<Expense> Data, int Total, int TotalDisplay)> GetPagedExpensesAsync(IGetExpenseListQuery request);

        IList<Expense> SearchByName(string query, int limit = 20);
    }
}
