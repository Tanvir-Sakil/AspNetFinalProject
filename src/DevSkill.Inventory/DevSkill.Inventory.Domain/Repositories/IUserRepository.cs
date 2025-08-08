using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Users.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IUserRepository 
    {
        Task<Guid> CreateUserAsync(string password, Guid employeeId, Guid roleId);
        Task<(IList<UserDto> Data, int Total, int TotalDisplay)> GetPagedUserAsync(IGetUserListQuery request);

        Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<User?> GetUserByIdAsync(Guid id);

        Task<bool> UpdateUserAsync(User user);
    }
}
