using G_NET_02_MVC01.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.DataSeed
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager
            , ILogger logger, CancellationToken cancellationToken)
        {
            try
            {
                bool HasUser = userManager.Users.Any();
                bool HasRole = roleManager.Roles.Any();
                if (HasRole && HasUser) return;
                if (!HasRole)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "SuperAdmin" },
                        new IdentityRole { Name = "Admin" }
                    };
                    foreach(var RoleName in Roles.Select(r=>r.Name)) 
                    { 
                        if(!await roleManager.RoleExistsAsync(RoleName))
                        {
                            var role = new IdentityRole { Name = RoleName };
                            var result = await roleManager.CreateAsync(role);
                            if (result.Succeeded)
                            {
                                logger.LogInformation($"Role '{RoleName}' created successfully.");
                            }
                            else
                            {
                                logger.LogError($"Error creating role '{RoleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                }
                if(!HasUser)
                {
                    var superAdminUser = new ApplicationUser
                    {
                        UserName = "Ahmed_Yasser",
                        FirstName = "Ahmed",
                        LastName =" Yasser",
                        PhoneNumber = "0126634877",
                        Email = "superadmin@gmail.com",
                    };
                    var result = await userManager.CreateAsync(superAdminUser, "P@ssw0rd!");
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"User '{superAdminUser.UserName}' created successfully.");
                    }
                    var adminUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        FirstName = "Admin",
                        LastName = "User",
                        PhoneNumber = "0123456789",
                        Email = "admin@gmail.com",
                    };
                    var result2 = await userManager.CreateAsync(adminUser, "P@ssw0rd!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (result2.Succeeded)
                    {
                        logger.LogInformation($"User '{adminUser.UserName}' created successfully.");
                    }
                 }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred during identity data seeding: {ex.Message}");
                throw;
            }
        }
    }
}
