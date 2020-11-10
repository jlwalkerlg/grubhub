using System;
using System.Threading.Tasks;

namespace FoodSnap.Application.Restaurants
{
    public interface IRestaurantDtoRepository
    {
        Task<RestaurantDto> GetById(Guid id);
    }
}
