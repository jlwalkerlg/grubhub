using System.Threading.Tasks;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<Coordinates>> LookupCoordinates(string postcode);
    }
}
