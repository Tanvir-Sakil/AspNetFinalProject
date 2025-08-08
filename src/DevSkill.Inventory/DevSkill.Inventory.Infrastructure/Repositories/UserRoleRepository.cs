using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DevSkill.Inventory.Domain.Features.Users.Queries;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UserRoleRepository : Repository<ApplicationRole,Guid>,IUserRoleRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleRepository(ApplicationDbContext dbContext, RoleManager<ApplicationRole> roleManager,
            IMapper mapper):base(dbContext)
        {
            _applicationDbContext = dbContext;
            _roleManager = roleManager;
            _mapper = mapper;

        }

        public async Task<(IList<RoleDto> Data, int Total, int TotalDisplay)> GetPagedUserRolesAsync(IGetUserRoleListQuery request)
        {
            Expression<Func<ApplicationRole, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.RoleName))
            {
                filter = filter.AndAlso(c => c.Name.Contains(request.RoleName));
            }

            if (!string.IsNullOrWhiteSpace(request.CompanyName))
            {
                filter = filter.AndAlso(c => c.CompanyName.Contains(request.CompanyName));
            }
            if (request.IsActive.HasValue)
            {
                filter = filter.AndAlso(c => c.IsActive == request.IsActive.Value);
            }
            if (request.CreateDateFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.CreatedDate >= request.CreateDateFrom.Value);
            }

            if (request.CreateDateTo.HasValue)
            {
                var endDate = request.CreateDateTo.Value.Date.AddDays(1).AddTicks(-1);
                filter = filter.AndAlso(c => c.CreatedDate <= endDate);
            }

            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                string searchTerm = request.Search.Value.Trim();
                filter = filter.AndAlso(c =>
                    c.Name.Contains(searchTerm) ||
                    c.CompanyName.Contains(searchTerm)
                );
            }

            var (data, total, totalDisplay) = await GetDynamicAsync(
                filter,
                request.OrderBy,
                null,
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true
            );

            var RoleDtos = data.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                CompanyName = role.CompanyName,
                CreatedDate = role.CreatedDate,
                IsActive = role.IsActive
            }).ToList();

            return (RoleDtos, total, totalDisplay);
        }


        public IList<RoleDto> SearchByName(string query, int limit = 20)
        {
            return _applicationDbContext.Set<ApplicationRole>()
                .Where(c => c.Name.Contains(query))
                .OrderBy(c => c.Name)
                .Take(limit)
                .Select(x => new RoleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CompanyName = x.CompanyName,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive
                })
                .ToList();
        }

        public async Task AddRoleAsync(UserRole role)
        {
            var appRole = _mapper.Map<ApplicationRole>(role);
            await _roleManager.CreateAsync(appRole);
        }

        public async Task<IList<RoleDto>> GetAllRolesAsync()
        {
            var roles = await Task.FromResult(_roleManager.Roles
                        .Select(r => new RoleDto
                        {
                            Id = r.Id,
                            Name = r.Name,
                            IsActive = r.IsActive
                        }).ToList());
            return roles;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _applicationDbContext.Roles.FindAsync(id);
            if (role == null)
                return false;

            _applicationDbContext.Roles.Remove(role);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<UserRole?> GetRoleByIdAsync(Guid id)
        {
            var entity = await _applicationDbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null) return null;

            return new UserRole(entity.Id, entity.Name, entity.IsActive,entity.ConcurrencyStamp);
        }

        public async Task<bool> UpdateRoleAsync(UserRole userRole)
        {
            var entity = await _applicationDbContext.Roles.FirstOrDefaultAsync(r => r.Id == userRole.Id);
            if (entity == null) return false;

            _mapper.Map(userRole, entity);
            return true;
        }

    }
}
