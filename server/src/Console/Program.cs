using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web;

namespace Console
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Web.Program.CreateHostBuilder(args);
            builder.UseContentRoot(Path.GetDirectoryName(typeof(Startup).Assembly.Location));
            builder.ConfigureServices((_, s) =>
            {
                s.AddScoped<Seeder>();
            });
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                if (args.Contains("seed"))
                {
                    var seeder = services.GetRequiredService<Seeder>();
                    await seeder.Seed();
                }
                else
                {
                    await Run(services);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private static async Task Run(IServiceProvider services)
        {
            await Task.CompletedTask;
        }
    }
}
