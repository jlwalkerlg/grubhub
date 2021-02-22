using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Geocoding
{
    public static class GeocodingRegistrar
    {
        public static void AddGeocoding(this IServiceCollection services)
        {
            services.AddSingleton<IGeocoder, GoogleGeocoder>();
        }
    }
}
