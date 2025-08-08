using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Categories.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICategoryRepository :IRepository<Category, Guid>
    {
        Task<(IList<Category> Data, int Total, int TotalDisplay)> GetPagedCategoriesAsync(IGetCategoryListQuery request);

        IList<Category> SearchByName(string query, int limit = 20);
    }
}
