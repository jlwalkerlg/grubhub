using System.Linq;
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
                if (settings.Cap.Storage.Driver == "PostgreSql")
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

                if (settings.Cap.Transport.Driver == "RabbitMq")
                {
                    x.UseRabbitMQ(options =>
                    {
                        options.HostName = settings.Cap.Transport.RabbitMq.HostName;
                        options.UserName = settings.Cap.Transport.RabbitMq.UserName;
                        options.Password = settings.Cap.Transport.RabbitMq.Password;
                        options.Port = settings.Cap.Transport.RabbitMq.Port;
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
