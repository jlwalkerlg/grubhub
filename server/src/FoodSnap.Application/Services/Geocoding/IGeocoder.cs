using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<Result<Coordinates>> GetCoordinates(Address address);
    }
}
