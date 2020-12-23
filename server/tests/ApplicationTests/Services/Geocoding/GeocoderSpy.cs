using System.Threading.Tasks;
using Application.Services.Geocoding;
using Application;

namespace ApplicationTests.Services.Geocoding
{
    public class GeocoderSpy : IGeocoder
    {
        public Result<GeocodingResult> Result { get; set; }
        public string SearchAddress { get; private set; }

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            SearchAddress = address;
            return Task.FromResult(Result);
        }
    }
}
