using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Services.Geocoding;

namespace WebTests.Doubles
{
    public class GeocoderSpy : IGeocoder
    {
        public Result<Coordinates> LookupCoordinatesResult { get; set; }

        public Task<Result<Coordinates>> LookupCoordinates(string postcode)
        {
            return Task.FromResult(LookupCoordinatesResult);
        }
    }
}
