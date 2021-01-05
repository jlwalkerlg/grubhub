using System.Linq;
using Web.Features.Restaurants;
using Web.Domain.Restaurants;

namespace SharedTests
{
    public static class TestExtensions
    {
        public static bool IsEqual(this Restaurant restaurant, RestaurantDto restaurantDto)
        {
            if (restaurant.Id.Value != restaurantDto.Id) return false;

            foreach (var cuisine in restaurant.Cuisines)
            {
                if (!restaurantDto.Cuisines.Any(x => x.Name == cuisine.Name))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
