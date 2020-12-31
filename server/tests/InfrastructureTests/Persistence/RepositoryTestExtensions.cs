using Application.Restaurants;
using Domain.Restaurants;
using Xunit;

namespace InfrastructureTests.Persistence
{
    public static class RepositoryTestExtensions
    {
        public static void AssertEqual(this RestaurantDto restaurantDto, Restaurant restaurant)
        {
            Assert.Equal(restaurantDto.Id, restaurant.Id.Value);
        }

        public static void AssertEqual(this Restaurant restaurant, RestaurantDto restaurantDto)
        {
            restaurantDto.AssertEqual(restaurant);
        }
    }
}
