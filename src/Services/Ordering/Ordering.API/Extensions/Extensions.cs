using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Ordering.API.Extensions
{
    public static class Extensions
    {
        public static void AddMigration<TContext>(this WebApplication webApp, Action<TContext, IServiceProvider> Seeder, int Retry = 0) where TContext : DbContext
        {

            using (var scope = webApp.Services.CreateScope())
            {
                var SP = scope.ServiceProvider;
                var logger = SP.GetRequiredService<ILogger<TContext>>();
                var context = SP.GetRequiredService<TContext>();
               
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    context.Database.Migrate();
                    SeedData(Seeder, context, SP);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogInformation("CONNECTION STRING ^^");
                    logger.LogInformation($"...>Connection String :  {context.Database.GetConnectionString()}");
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                    if (Retry < 3)
                    {
                        Retry++;
                        Thread.Sleep(1000);
                        AddMigration(webApp, Seeder, Retry);
                    }
                }
            }
        }
        private static void SeedData<TContext>(Action<TContext, IServiceProvider> Seeder, TContext Context, IServiceProvider Services) where TContext : DbContext
        {
            Seeder(Context, Services);
        }
    }
}
