using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Console
{
    class Program
    {
        private static readonly Dictionary<string, string> Usage = new()
        {
            { "seed", "seed" },
        };

        public static async Task Main(string[] args)
        {
            if (args.Length == 0 || !Usage.ContainsKey(args[0]))
            {
                System.Console.WriteLine("USAGE:");
                System.Console.WriteLine("    dotnet run <command>");
                System.Console.WriteLine();

                System.Console.WriteLine("COMMANDS:");
                foreach (var arg in Usage)
                {
                    System.Console.WriteLine($"    {arg.Value}");
                }
                System.Console.WriteLine();

                return;
            }

            var builder = Web.Program.CreateHostBuilder(args);
            builder.ConfigureServices((ctx, s) =>
            {
                s.AddScoped<DbSeeder>();
            });
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                if (args.Contains("seed"))
                {
                    await Seed(services);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private static async Task Seed(IServiceProvider services)
        {
            var seeder = services.GetRequiredService<DbSeeder>();

            await seeder.Seed();
        }
    }
}
