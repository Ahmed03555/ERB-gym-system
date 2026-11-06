using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.DataSeed;
using G_NET_02_MVC01.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace G_NET_02_MVC01
{
    public static class ProgramExtentions
    {
        public static async Task MigrationAndSeedAsync(this WebApplication app)
        {
          using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying migrations...");
                await dbcontext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations found.");
            }
            var SeedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            await GymDataSeed.SeedData(dbcontext, SeedPath, logger, CancellationToken.None);
            await IdentityDataSeeding.SeedAsync(RoleManager, UserManager, logger, CancellationToken.None);
        }
    }
    
}
