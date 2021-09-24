using System.Linq;
using Amazon;
using Amazon.Runtime;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Web.Services.Events
{
    public static class CapRegistrar
    {
        public static void AddCap(this IServiceCollection services, Settings settings)
        {
            var subscribers = typeof(Startup).Assembly.GetTypes()
                .Where(x => !x.IsAbstract
                            && !x.IsInterface
                            && x.GetInterfaces().Any(i => i == typeof(ICapSubscribe)));

            foreach (var subscriber in subscribers)
            {
                services.AddScoped(subscriber);
            }

            services.AddCap(x =>
            {
                if (settings.Cap.Storage.Driver == CapSettings.StorageSettings.StorageDriver.PostgreSql)
                {
                    x.UsePostgreSql(options =>
                    {
                        options.ConnectionString = settings.Database.ConnectionString;
                    });
                }
                else
                {
                    x.UseInMemoryStorage();
                }

                if (settings.Cap.Transport.Driver == CapSettings.TransportSettings.TransportDriver.AmazonSQS)
                {
                    x.UseAmazonSQS(options =>
                    {
                        options.Region = RegionEndpoint.GetBySystemName(settings.Aws.Region);
                        options.Credentials = new BasicAWSCredentials(settings.Aws.AccessKeyId, settings.Aws.SecretAccessKey);
                    });
                }
                else if (settings.Cap.Transport.Driver == CapSettings.TransportSettings.TransportDriver.AzureServiceBus)
                {
                    x.UseAzureServiceBus(options =>
                    {
                        options.ConnectionString = settings.Azure.ServiceBus.ConnectionString;
                    });
                }
                else
                {
                    x.UseInMemoryMessageQueue();
                }

                x.UseDashboard();
            });
        }
    }
}
