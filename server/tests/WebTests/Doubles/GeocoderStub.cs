using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Services.Geocoding;

namespace WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public static float Latitude { get; } = 54.0f;
        public static float Longitude { get; } = -2.0f;

        public Task<Result<Coordinates>> LookupCoordinates(string postcode)
        {
            return Task.FromResult(Result.Ok(
                new Coordinates(Latitude, Longitude)));
        }
    }
}
