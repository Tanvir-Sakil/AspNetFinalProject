using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IStaffRepository : IRepository<Staff, Guid>
    {
        Task<string> GenerateNextStaffCodeAsync();
    }
}
