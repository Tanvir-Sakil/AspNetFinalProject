using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ISaleTypeRepository : IRepository<SaleType, Guid>
    {
        Task<List<SaleType>> GetAllSalesTypesAsync();
    }
}
