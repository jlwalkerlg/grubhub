using System;
using System.IO;
using System.Linq;
using Application.Services.Hashing;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Persistence.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    System.Console.WriteLine(e.ToString());
                }
            }
            else
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
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

                var context = sp.GetService<AppDbContext>();
                var hasher = sp.GetService<IHasher>();

                var seeder = new DbSeeder(context, hasher);
                seeder.Seed().Wait();
            }
        }
    }
}
