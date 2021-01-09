using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain;

namespace Web.Features.Restaurants
{
    public interface IRestaurantDtoRepository
    {
        Task<List<RestaurantDto>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null);
    }

    public record RestaurantSearchOptions
    {
        public string SortBy { get; set; }
        public List<string> Cuisines { get; set; } = new();
    }
}
