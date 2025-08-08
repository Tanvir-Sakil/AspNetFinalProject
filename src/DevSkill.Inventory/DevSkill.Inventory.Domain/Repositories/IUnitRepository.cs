using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Units.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IUnitRepository :IRepository<DevSkill.Inventory.Domain.Entities.Unit, Guid>
    {
        Task<(IList<DevSkill.Inventory.Domain.Entities.Unit> Data, int Total, int TotalDisplay)> GetPagedUnitsAsync(IGetUnitListQuery request);

        IList<Unit> SearchByName(string query, int limit = 20);

        Task<Unit?> GetByNameAsync(string name);
    }
}
