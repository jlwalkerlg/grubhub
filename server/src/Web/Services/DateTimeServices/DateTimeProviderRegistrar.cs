using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.DateTimeServices
{
    public static class DateTimeProviderRegistrar
    {
        public static void AddDateTimeProvider(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
