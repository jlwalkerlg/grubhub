using Autofac;
using FoodSnap.Application.Services.Geocoding;
using System.Threading.Tasks;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class GeocoderRegistrar
    {
        public static void AddGeocoder(this ContainerBuilder builder)
        {
            // TODO: register actual implementation
            builder.RegisterType<GeocodingServiceStub>()
                .As<IGeocoder>()
                .InstancePerLifetimeScope();
        }
    }

    public class GeocodingServiceStub : IGeocoder
    {
        public Task<CoordinatesDto> GetCoordinates(AddressDto address)
        {
            return Task.FromResult(new CoordinatesDto
            {
                Latitude = 0,
                Longitude = 0
            });
        }
    }
}
