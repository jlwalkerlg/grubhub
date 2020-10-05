using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Domain;

namespace FoodSnap.WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            return Task.FromResult(Result.Ok(new GeocodingResult
            {
                FormattedAddress = "1 Maine Road, Manchester, MN121NM, UK",
                Latitude = 1,
                Longitude = 1
            }));
        }
    }
}
