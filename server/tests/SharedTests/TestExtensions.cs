using Application.Restaurants;
using Domain.Restaurants;

namespace SharedTests
{
    public static class TestExtensions
    {
        public static bool IsEqual(this Restaurant restaurant, RestaurantDto restaurantDto)
        {
            return restaurant.Id.Value == restaurantDto.Id;
        }
    }
}
