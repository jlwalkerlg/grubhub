using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Web.Data.EF;
using Web.Services.Hashing;

namespace Console
{
    class Program
    {
        private static readonly Dictionary<string, string> usage = new()
        {
            { "seed", "seed" },
            { "create-stripe-account", "create-stripe-account [--name <name>]" },
            { "generate-onboarding-link", "generate-onboarding-link <accountId>" },
            { "create-payment-intent", "create-payment-intent" },
        };

        static void Main(string[] args)
        {
            if (args.Length == 0 || !usage.ContainsKey(args[0]))
            {
                System.Console.WriteLine("USAGE:");
                System.Console.WriteLine("    dotnet run <command>");
                System.Console.WriteLine();

                System.Console.WriteLine("COMMANDS:");
                foreach (var arg in usage)
                {
                    System.Console.WriteLine($"    {arg.Value}");
                }
                System.Console.WriteLine();

                return;
            }

            var builder = Web.Program.CreateHostBuilder(args);
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                if (args.Contains("seed"))
                {
                    Seed(services).Wait();
                }
                else if (args.Contains("create-stripe-account"))
                {
                    CreateConnectStripeAccount(args).Wait();
                }
                else if (args.Contains("generate-onboarding-link") && args.Length > 1)
                {
                    GenerateOnboardingLink(args[1]).Wait();
                }
                else if (args.Contains("create-payment-intent"))
                {
                    CreatePaymentIntent().Wait();
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private static async Task Seed(IServiceProvider services)
        {
            var seeder = new DbSeeder(
                services.GetRequiredService<AppDbContext>(),
                services.GetRequiredService<IHasher>()
            );

            await seeder.Seed();
        }

        private static async Task CreateConnectStripeAccount(string[] args)
        {
            var service = new AccountService();

            string name = null;
            if (args.Contains("--name"))
            {
                var index = args.ToList().FindIndex(x => x == "--name");

                if (args.Length > index)
                {
                    name = args[index + 1];
                }
            }

            var account = await service.CreateAsync(
                new AccountCreateOptions()
                {
                    Type = "express",
                    BusinessType = "company",
                    Company = new AccountCompanyOptions()
                    {
                        Name = name,
                    },
                }
            );


            System.Console.WriteLine("Account ID: " + account.Id);
        }

        private static async Task GenerateOnboardingLink(string accountId)
        {
            var service = new AccountLinkService();

            var link = await service.CreateAsync(
                new AccountLinkCreateOptions()
                {
                    Account = accountId,
                    RefreshUrl = $"http://localhost:5000/stripe/onboarding/refresh",
                    ReturnUrl = "http://localhost:3000/dashboard/billing",
                    Type = "account_onboarding",
                }
            );

            System.Console.WriteLine("Onboarding link " + link.Url);
        }

        private static async Task CreatePaymentIntent()
        {
            var service = new PaymentIntentService();

            var intent = await service.CreateAsync(
                new PaymentIntentCreateOptions()
                {
                    CaptureMethod = "manual",
                    PaymentMethodTypes = new List<string>() { "card" },
                    Amount = 2000,
                    Currency = "gbp",
                    ApplicationFeeAmount = 50,
                    TransferData = new PaymentIntentTransferDataOptions()
                    {
                        Destination = "acct_1IIDXKPRU0NZyTXU",
                    },
                }
            );

            System.Console.WriteLine(intent.ClientSecret);
        }
    }
}
