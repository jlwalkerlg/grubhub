using System.Threading;
using System;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Restaurants.GetRestaurantById;
using Xunit;
using static FoodSnap.Shared.Error;

namespace FoodSnap.ApplicationTests.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdHandlerTests
    {
        private readonly RestaurantDtoRepositoryFake restaurantDtoRepositoryFake;
        private readonly GetRestaurantByIdHandler handler;

        public GetRestaurantByIdHandlerTests()
        {
            restaurantDtoRepositoryFake = new RestaurantDtoRepositoryFake();

            handler = new GetRestaurantByIdHandler(restaurantDtoRepositoryFake);
        }

        [Fact]
        public async Task It_Returns_The_Restaurant()
        {
            var restaurant = new RestaurantDto
            {
                Id = Guid.NewGuid(),
            };
            restaurantDtoRepositoryFake.Restaurants.Add(restaurant);

            var query = new GetRestaurantByIdQuery { Id = restaurant.Id };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Same(restaurant, result.Value);
        }

        [Fact]
        public async Task It_Fails_If_Restaurant_Not_Found()
        {
            var query = new GetRestaurantByIdQuery { Id = Guid.NewGuid() };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}