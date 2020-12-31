using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Restaurants
{
    public interface IRestaurantDtoRepository
    {
        Task<RestaurantDto> GetById(Guid id);

        Task<List<RestaurantDto>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null);
    }

    public record RestaurantSearchOptions
    {
        public string SortBy { get; set; }
    }
}
