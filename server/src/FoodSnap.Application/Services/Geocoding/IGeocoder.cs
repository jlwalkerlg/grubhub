using System.Threading.Tasks;
namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingResult>> Geocode(string address);
    }
}
