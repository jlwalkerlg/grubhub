using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Events
{
    public static class EventBusRegistrar
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, EFEventBus>();
        }

        public static void AddEventWorker(this IServiceCollection services)
        {
            services.AddHostedService<EventWorker>();
        }
    }
}
