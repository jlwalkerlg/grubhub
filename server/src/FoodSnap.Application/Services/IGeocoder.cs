using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Services
{
    public interface IGeocoder
    {
        Task<Coordinates> GetCoordinates(Address address);
    }
}
