using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                // .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Product.View, policy =>
                    policy.RequireClaim("Permission", Permissions.Product.View));

                options.AddPolicy(Permissions.Product.Add, policy =>
                     policy.RequireClaim("Permission", Permissions.Product.Add));

                options.AddPolicy(Permissions.Product.Create, policy =>
                    policy.RequireClaim("Permission", Permissions.Product.Create));

                options.AddPolicy(Permissions.Product.Edit, policy =>
                    policy.RequireClaim("Permission", Permissions.Product.Edit));

                options.AddPolicy(Permissions.Product.Delete, policy =>
                    policy.RequireClaim("Permission", Permissions.Product.Delete));

                options.AddPolicy(Permissions.Customer.View, policy =>
                    policy.RequireClaim("Permission", Permissions.Customer.View));

                options.AddPolicy(Permissions.Customer.Create, policy =>
                    policy.RequireClaim("Permission", Permissions.Customer.Create));

                options.AddPolicy(Permissions.Customer.Edit, policy =>
                    policy.RequireClaim("Permission", Permissions.Customer.Edit));

                options.AddPolicy(Permissions.Customer.Delete, policy =>
                    policy.RequireClaim("Permission", Permissions.Customer.Delete));

                options.AddPolicy(Permissions.Sales.View, policy =>
                    policy.RequireClaim("Permission", Permissions.Sales.View));

                options.AddPolicy(Permissions.Sales.Create, policy =>
                    policy.RequireClaim("Permission", Permissions.Sales.Create));

                options.AddPolicy(Permissions.Sales.Edit, policy =>
                    policy.RequireClaim("Permission", Permissions.Sales.Edit));

                options.AddPolicy(Permissions.Sales.Delete, policy =>
                    policy.RequireClaim("Permission", Permissions.Sales.Delete));


                options.AddPolicy(Permissions.BalanceTransfer.View, policy =>
                    policy.RequireClaim("Permission", Permissions.BalanceTransfer.View));

                options.AddPolicy(Permissions.BalanceTransfer.Create, policy =>
                    policy.RequireClaim("Permission", Permissions.BalanceTransfer.Create));

                options.AddPolicy(Permissions.BalanceTransfer.Edit, policy =>
                    policy.RequireClaim("Permission", Permissions.BalanceTransfer.Edit));

                options.AddPolicy(Permissions.BalanceTransfer.Delete, policy =>
                    policy.RequireClaim("Permission", Permissions.BalanceTransfer.Delete));
            });
        }
    }
}