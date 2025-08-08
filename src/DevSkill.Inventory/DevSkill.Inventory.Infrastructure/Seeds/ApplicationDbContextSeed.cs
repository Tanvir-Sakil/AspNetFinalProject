using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string roleName = "SuperAdmin";
            var superAdminRole = await roleManager.FindByNameAsync(roleName);
            if (superAdminRole == null)
            {
                superAdminRole = new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    CompanyName = "BikreeBee"
                };
                await roleManager.CreateAsync(superAdminRole);
            }
            var employee = await context.Staffs.FirstOrDefaultAsync(e => e.Email == "SuperAdmin@gmail.com");
            if (employee == null)
            {
                employee = new Staff
                {
                    Id = Guid.NewGuid(),
                    EmployeeName = "Super Admin",
                    StaffCode = "S0001",
                    Email = "SuperAdmin@gmail.com",
                    Phone = "01700000000",
                    Address = "Dhaka,Bangladesh",
                    JoiningDate = DateTime.UtcNow,
                    Nid = "1234567890",
                    IsActive = true
                };

                context.Staffs.Add(employee);
                await context.SaveChangesAsync();
            }
            var superAdminUser = await userManager.FindByEmailAsync("SuperAdmin@gamil.com");
            var superAdminUserByEmail = await userManager.FindByEmailAsync("SuperAdmin@gmail.com");
            var superAdminUserByName = await userManager.FindByNameAsync("SuperAdmin");

            if (superAdminUserByEmail == null && superAdminUserByName == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01700000000",
                    EmailConfirmed = true,
                    EmployeeId = employee.Id,
                    RoleId = superAdminRole.Id,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Admin@123"); 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception("Super Admin creation failed: " + errors);
                }
            }
        }
    }

}
