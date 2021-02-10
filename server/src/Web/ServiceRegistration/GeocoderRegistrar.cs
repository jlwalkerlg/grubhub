using Autofac;
using Web.Services.Geocoding;

namespace Web.ServiceRegistration
{
    public static class GeocoderRegistrar
    {
        public static void AddGeocoder(this ContainerBuilder builder)
        {
            builder
                .RegisterType<GoogleGeocoder>()
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
