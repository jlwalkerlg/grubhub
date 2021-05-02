using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Cache
{
    public static class CacheRegistrar
    {
        public static void AddDistributedCache(this IServiceCollection services, CacheSettings settings)
        {
            if (settings.Driver == "Redis")
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = settings.Redis.ConnectionString;
                    options.InstanceName = settings.Redis.InstanceName;
                });

                return;
            }

            services.AddDistributedMemoryCache();
        }
    }
}
