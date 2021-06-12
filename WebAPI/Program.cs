using Entities;
using Entities.Initializer;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Areas.Identity.Data;
using WebAPI.Data;

namespace WebAPI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILoggerManager>();
                var context = services.GetRequiredService<PropertyContext>();
                context.Database.EnsureCreated();
                try
                {
                    var userContext = services.GetRequiredService<IdentityContext>();
                    userContext.Database.EnsureCreated();
                    var userManager = services.GetRequiredService<UserManager<WebAPIUser>>();
                    // Adding users to the Identity database and then adding these to the Property database
                    var users = await IdentityInitializer.AddUsersAsync(userManager);
                    IdentityInitializer.AddUsersToEstateDb(context, users);
                    logger.LogInfo("IdentityDatabase initialized");
                }
                catch (Exception Ex)
                {
                    logger.LogError($"Error when initializing the identity database");
                    logger.LogDebug($"{Ex}");
                }
                try
                {
                    // Adding the EstateDatabase and filling it with data
                    RealEstateInitializer.Initialize(context, logger);
                    logger.LogInfo("Database initialized");
                }
                catch (Exception Ex)
                {
                    logger.LogError($"Error when initializing the database");
                    logger.LogDebug($"{Ex}");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
