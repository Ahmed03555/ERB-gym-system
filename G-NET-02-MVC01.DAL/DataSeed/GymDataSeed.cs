using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G_NET_02_MVC01.DAL.DataSeed
{
    public static class GymDataSeed
    { 

    public static async Task SeedData(GymContext context, string seedFilePath,ILogger logger,CancellationToken cancellationToken)
        {
            try
            {
                if(!await context.plans.AnyAsync(cancellationToken))
                {
                   var plans = LoadDateFromJson<Plans>("plans.json", seedFilePath);
                   if(plans.Count > 0) {

                        context.plans.AddRange(plans);
                        logger.LogInformation($"Seeded {plans.Count} plans.");
                    }
                }
                if(context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync(cancellationToken);
                    logger.LogInformation("Database changes saved successfully.");
                }
                else
                {
                    logger.LogInformation("No changes detected in the database. No seeding performed.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }




        private static List<T> LoadDateFromJson<T>(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{fileName}' was not found in the folder '{folderPath}'.");
            }
            var jsonData = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(jsonData, options) ?? [];
        }
    }
}
