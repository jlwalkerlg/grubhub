using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Geocoding
{
    public static class GeocodingRegistrar
    {
        public static void AddGeocoding(this IServiceCollection services, GeocodingSettings settings)
        {
            if (settings.Driver == "Google")
            {
                services.AddHttpClient<IGeocoder, GoogleGeocoder>();
            }
            else
            {
                services.AddSingleton<IGeocoder, DoogalGeocoder>();
            }
        }
    }
}
