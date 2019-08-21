using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace MicroServicesDemo.Extensions
{
    /// <summary>
    /// Extensions to the Asp.Net web host used to start your hosting environment
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Used during debugging locally, this will create and migrate your database
        /// </summary>
        /// <param name="webHost">The Asp.Net host</param>
        /// <returns>The provided web host</returns>
        public static IWebHost RegisterDefaultJson(this IWebHost webHost)
        {
            var settings = webHost.Services.GetService<JsonSerializerSettings>();
            JsonConvert.DefaultSettings = () => settings;
            return webHost;
        }

        /// <summary>
        /// Used during debugging locally, this will create and migrate your database
        /// </summary>
        /// <param name="webHost">The Asp.Net host</param>
        /// <returns>The provided web host</returns>
        public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T: DbContext
        {
            var environment = webHost.Services.GetService<IHostingEnvironment>();
            if (!environment.IsDevelopment()) 
                return webHost;

            try
            {
                var serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = serviceScopeFactory.CreateScope())
                using (var context = scope.ServiceProvider.GetRequiredService<T>())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            catch
            {
                //Do not let the failure of the database at startup keep the application from starting
                if (DebugSettings.IsDebugging)
                {
                    throw;
                }
            }

            return webHost;
        }
    }
}