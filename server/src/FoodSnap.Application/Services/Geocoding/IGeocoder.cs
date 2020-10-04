using System.Threading.Tasks;
using FoodSnap.Domain;

namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<GeocodingData>> Geocode(string address);
    }
}
