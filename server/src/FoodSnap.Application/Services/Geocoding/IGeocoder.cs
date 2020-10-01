using System.Threading.Tasks;

namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingData>> Geocode(string address);
    }
}
