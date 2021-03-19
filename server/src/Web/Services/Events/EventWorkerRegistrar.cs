using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Events
{
    public static class EventWorkerRegistrar
    {
        public static void AddEventWorker(this IServiceCollection services)
        {
            services.AddHostedService<EventWorker>();
        }
    }
}
