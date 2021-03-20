using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Events
{
    public static class EventProcessorRegistrar
    {
        public static void AddEventProcessor(this IServiceCollection services)
        {
            services.AddHostedService<EventProcessor>();
        }
    }
}
