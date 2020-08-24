using Autofac;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Infrastructure.Geocoding;
using Microsoft.Extensions.Configuration;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class GeocoderRegistrar
    {
        public static void AddGeocoder(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder
                .Register(ctx =>
                {
                    return new GoogleGeocoder(configuration["GoogleGeocodingApiKey"]);
                })
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
