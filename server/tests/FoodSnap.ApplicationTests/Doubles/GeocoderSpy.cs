using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;

namespace FoodSnap.ApplicationTests.Doubles.GeocoderSpy
{
    public class GeocoderSpy : IGeocoder
    {
        public GeocodingData Data { get; set; }
        public string SearchAddress { get; private set; }

        public Task<Result<GeocodingData>> Geocode(string address)
        {
            SearchAddress = address;
            return Task.FromResult(Result.Ok(Data));
        }
    }
}
