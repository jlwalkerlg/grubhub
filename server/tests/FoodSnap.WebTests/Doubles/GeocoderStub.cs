using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;

namespace FoodSnap.WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public Task<Result<GeocodingData>> Geocode(string address)
        {
            return Task.FromResult(Result.Ok(new GeocodingData
            {
                FormattedAddress = "1 Maine Road, Manchester, MN121NM, UK",
                Latitude = 1,
                Longitude = 1
            }));
        }
    }
}
