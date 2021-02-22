using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Services.Geocoding;

namespace WebTests.Doubles
{
    public class GeocoderSpy : IGeocoder
    {
        public Result<GeocodingResult> GeocodeResult { get; set; }
        public Result<Coordinates> LookupCoordinatesResult { get; set; }

        public string SearchAddress { get; private set; }

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            SearchAddress = address;
            return Task.FromResult(GeocodeResult);
        }

        public Task<Result<Coordinates>> LookupCoordinates(string postcode)
        {
            return Task.FromResult(LookupCoordinatesResult);
        }
    }
}
