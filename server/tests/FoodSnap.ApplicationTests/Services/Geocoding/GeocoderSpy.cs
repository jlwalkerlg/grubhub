using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Application;

namespace FoodSnap.ApplicationTests.Services.Geocoding
{
    public class GeocoderSpy : IGeocoder
    {
        public GeocodingResult Data { get; set; }
        public string SearchAddress { get; private set; }

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            SearchAddress = address;
            return Task.FromResult(Result.Ok(Data));
        }
    }
}
