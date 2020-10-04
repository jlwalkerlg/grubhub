using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Domain;

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
