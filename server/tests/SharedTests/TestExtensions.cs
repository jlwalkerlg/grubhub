using Application.Restaurants;
using Domain.Restaurants;
using Xunit;

namespace SharedTests
{
    public static class TestExtensions
    {
        public static void AssertEqual(this Restaurant restaurant, RestaurantDto restaurantDto)
        {
            Assert.Equal(restaurant.Id.Value, restaurantDto.Id);
        }
    }
}
