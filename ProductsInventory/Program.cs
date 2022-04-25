using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsInventory.Seed;
using System;

namespace ProductsInventory
{
    /// <summary>
    /// Program .cs
    /// </summary>
    public class Program
    {
        /// <summary>
        ///  Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                    databaseInitializer.SeedAsync().Wait();
                    
                }
                catch (Exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    //logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);
                    throw;
                }

            }
            host.Run();
        }
        /// <summary>
        /// host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
