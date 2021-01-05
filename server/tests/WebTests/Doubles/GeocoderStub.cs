using System.Threading.Tasks;
using Web.Services.Geocoding;
using Web;
using Web.Domain;

namespace WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public static string Address { get; } = "1 Maine Road, Manchester, MN121NM, UK";
        public static float Latitude { get; } = 54.0f;
        public static float Longitude { get; } = -2.0f;

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            return Task.FromResult(Result.Ok(new GeocodingResult()
            {
                FormattedAddress = Address,
                Coordinates = new Coordinates(Latitude, Longitude),
            }));
        }
    }
}
