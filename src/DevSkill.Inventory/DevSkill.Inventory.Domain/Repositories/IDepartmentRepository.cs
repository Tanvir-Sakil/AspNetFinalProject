using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Departments.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IDepartmentRepository :IRepository<Department, Guid>
    {
        Task<(IList<Department> Data, int Total, int TotalDisplay)> GetPagedDepartmentsAsync(IGetDepartmentListQuery request);

        IList<Department> SearchByName(string query, int limit = 20);
    }
}
