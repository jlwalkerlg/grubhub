using System.Threading.Tasks;
using Web;
using Web.Services.Geocoding;

namespace WebTests.Doubles
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
