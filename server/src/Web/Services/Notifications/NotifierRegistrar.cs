using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Notifications
{
    public static class NotifierRegistrar
    {
        public static void AddNotifier(this IServiceCollection services)
        {
            services.AddSingleton<INotifier, HubNotifier>();
        }
    }
}
