using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.UserRoles.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IUserRoleRepository 
    {
        Task<(IList<RoleDto> Data, int Total, int TotalDisplay)> GetPagedUserRolesAsync(IGetUserRoleListQuery request);

        IList<RoleDto> SearchByName(string query, int limit = 20);

        // Task<bool> AddRoleAsync(string roleName, string? companyName = null);

        Task AddRoleAsync(UserRole role);

        Task<UserRole?> GetRoleByIdAsync(Guid id);
     //   Task UpdateAsync(UserRole userRole);

        Task<bool> UpdateRoleAsync(UserRole role);

        Task<IList<RoleDto>> GetAllRolesAsync();

        Task<bool> DeleteRoleAsync(Guid id);
    }
}
