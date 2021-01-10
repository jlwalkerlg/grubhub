using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Web.Data;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            if (args.Contains("--seed"))
            {
                try
                {
                    hostBuilder.ConfigureServices(services =>
                        services.AddTransient<DbSeeder>());
                    var host = hostBuilder.Build();
                    Seed(host);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            else
            {
                hostBuilder.Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddConfiguration(
                        new ConfigurationBuilder()
                            .AddJsonFile("config.json")
                            .Build()
                    );
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void Seed(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;

                var seeder = sp.GetRequiredService<DbSeeder>();
                seeder.Seed().Wait();
            }
        }
    }
}
