using Autofac;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Infrastructure.Geocoding;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class GeocoderRegistrar
    {
        public static void AddGeocoder(this ContainerBuilder builder, WebConfig config)
        {
            builder
                .Register(ctx => new GoogleGeocoder(config.GoogleGeocodingApiKey))
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
