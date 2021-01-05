using System.Threading.Tasks;
namespace Web.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingResult>> Geocode(string address);
    }
}
