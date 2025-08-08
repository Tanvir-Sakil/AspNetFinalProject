using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IUserPermissionRepository
    {
        Task<List<string>> GetPermissionsForUserAsync(Guid userId);
        Task AddPermissionToUserAsync(Guid userId, string permission);
        Task RemoveAllPermissionsForUserAsync(Guid userId);
    }
}
