using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using T2BootstrapServer.Entity.Model;

namespace T2BootstrapServer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
         var host = CreateHostBuilder(args).Build() ;
            using (var scope = host.Services.CreateScope() )
            {
                var services = scope.ServiceProvider;

                try
                {
                  // var context = services.GetRequiredService<ApplicationDbContext>();
                  // context.Database.Migrate();

                    var userManager = services.GetService<ILog>();
                    var dd = " ;";
                  //  await ApplicationDbContextSeed.SeedAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {


            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                  }).ConfigureLogging((hostingContext, logging) =>
                  {
                      logging.ClearProviders();
                      logging.AddLog4Net();
                      logging.SetMinimumLevel(LogLevel.Trace);
                  }).ConfigureServices((host, services) =>
                  {

                      services.AddOptions(); 

                  });

        } 
    }
}
