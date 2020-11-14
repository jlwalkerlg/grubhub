using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingResult>> Geocode(string address);
    }
}
