using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Clocks
{
    public static class ClockRegistrar
    {
        public static void AddClock(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
        }
    }
}
