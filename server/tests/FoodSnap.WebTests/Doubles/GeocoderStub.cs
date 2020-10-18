using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Shared;

namespace FoodSnap.WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public static string Address { get; } = "1 Maine Road, Manchester, MN121NM, UK";
        public static int Latitude { get; } = 1;
        public static int Longitude { get; } = 2;

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            return Task.FromResult(Result.Ok(new GeocodingResult
            {
                FormattedAddress = Address,
                Latitude = Latitude,
                Longitude = Longitude,
            }));
        }
    }
}
