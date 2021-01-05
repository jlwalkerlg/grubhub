using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.ResolveAnything;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using Web.Data;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Contains("--seed"))
            {
                try
                {
                    Seed(host);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            else
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                {
                    builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                }))
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var solutionRootPath = Path.Combine(env.ContentRootPath, "..", "..");

                    config.AddJsonFile(Path.Combine(solutionRootPath, "config.json"));
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
