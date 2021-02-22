using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Hashing
{
    public static class HashingRegistrar
    {
        public static void AddHashing(this IServiceCollection services)
        {
            services.AddSingleton<IHasher, Hasher>();
        }
    }
}
