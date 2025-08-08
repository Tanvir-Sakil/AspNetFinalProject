using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<UserPermissionRepository> _logger;

        public UserPermissionRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext,ILogger<UserPermissionRepository> logger)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<List<string>> GetPermissionsForUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
        }

        public async Task AddPermissionToUserAsync(Guid userId, string permission)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("User not found");

            user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

            if (user == null)
                throw new Exception("User not found on reload");

            var existsInDb = await _applicationDbContext.Users.AnyAsync(u => u.Id == user.Id);
            if (!existsInDb)
                throw new Exception("User is in memory but not found in AspNetUsers table");

            //var result =await _userManager.AddClaimAsync(user, new Claim("Permission", permission));

            // if (!result.Succeeded)
            //     throw new Exception("Failed to add claim: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var existingClaims = await _userManager.GetClaimsAsync(user);
            if (existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
            {
                _logger.LogInformation($"Permission '{permission}' already exists for user '{userId}'");
                return; 
            }
            try
            {
                var result = await _userManager.AddClaimAsync(user, new Claim("Permission", permission));

                if (!result.Succeeded)
                {
                    var errorMessage = "Failed to add claim: " + string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while adding claim");
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while assigning permission");
                throw;
            }

        }

        public async Task RemoveAllPermissionsForUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var claims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in claims.Where(c => c.Type == "Permission"))
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
        }
    }
}
