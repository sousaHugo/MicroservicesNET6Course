using Discount.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection Services)
        {
            Services.AddScoped<IDiscountRepository, DiscountRepository>();
            return Services;
        }
        public static IApplicationBuilder MigrateDatabase<TContext>(this IApplicationBuilder Builder, int? Retry = 0)
        {
            var retryForAvailability = Retry.Value;

            using (var scope = Builder.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migration started!");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand()
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS \"Coupon\"";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS public.\"Coupon\"\r\n(\r\n    \"Id\" serial NOT NULL,\r\n    \"ProductName\" character varying(24) COLLATE pg_catalog.\"default\" NOT NULL,\r\n    \"Description\" text COLLATE pg_catalog.\"default\",\r\n    \"Amount\" integer NOT NULL,\r\n    CONSTRAINT \"Coupon_pkey\" PRIMARY KEY (\"Id\")\r\n)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO public.\"Coupon\"(\"ProductName\", \"Description\", \"Amount\")\tVALUES ('iPhone X', 'Apple Iphone', 10);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migration ended successfully");
                
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error as occurred migrating the database.");
                    if(retryForAvailability < 10)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(Builder, Retry);
                    }
                    throw;
                }
            }
            return Builder;
        }
    }

}
