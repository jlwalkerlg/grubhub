using System.Threading.Tasks;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingResult>> Geocode(string address);
        Task<Result<GeocodingResult>> Geocode(AddressDetails address);
    }
}
