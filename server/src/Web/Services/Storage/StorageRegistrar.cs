using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Storage
{
    public static class StorageRegistrar
    {
        public static void AddImageStorage(this IServiceCollection services)
        {
            services.AddSingleton<IImageStore, S3ImageStore>();
        }
    }
}
