using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mero_Doctor_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mero_Doctor_Project.Helper
{
    public static class AppInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetService<ILoggerFactory>()?.CreateLogger("Mero_Doctor_Project.Helper.AppInitializer");

            try
            {
                // Create roles
                string[] roles = { "Patient", "Doctor", "Admin" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!result.Succeeded)
                        {
                            logger?.LogError("Failed to create role {Role}: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                }

                // Create default admin user
                var adminUser = await userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admin@health.com",
                        FullName = "Super Admin"
                    };
                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        logger?.LogInformation("Admin user created successfully.");
                    }
                    else
                    {
                        logger?.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred during application initialization.");
                throw;
            }
        }
    }
}