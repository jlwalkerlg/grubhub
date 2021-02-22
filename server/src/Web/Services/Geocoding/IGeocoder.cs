using System.Threading.Tasks;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingResult>> Geocode(string address);
        Task<Result<Coordinates>> LookupCoordinates(string postcode);
    }
}
