using MicroServiceDemo.Api.Blog.Data;
using MicroServicesDemo;
using MicroServicesDemo.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroServiceDemo.Api.Blog
{
    /// <summary>
    /// Main Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Any arguments provided by the command line</param>
        public static void Main(string[] args) =>
           CreateWebHostBuilder(args)
                .Build()
                .MigrateDatabase<BlogDbContext>()
                .RegisterDefaultJson()
                .Run();

        /// <summary>
        /// The method called by entity framework and other libraries to simulate starting the application, this method should not be renamed
        /// </summary>
        /// <param name="args">Any arguments provided by the command line</param>
        /// <returns>The web host builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                    //Only add debugging and console while debugging the application
                    if (DebugSettings.IsDebugging)
                    {
                        logging.AddDebug();
                        logging.AddConsole();
                    }

                    logging.AddApplicationInsights();
                })
                .UseStartup<Startup>();
    }
}
