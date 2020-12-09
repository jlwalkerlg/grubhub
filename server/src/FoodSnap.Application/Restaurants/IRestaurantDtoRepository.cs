using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Domain;

namespace FoodSnap.Application.Restaurants
{
    public interface IRestaurantDtoRepository
    {
        Task<RestaurantDto> GetById(Guid id);
        Task<List<RestaurantDto>> Search(Coordinates coordinates);
    }
}
