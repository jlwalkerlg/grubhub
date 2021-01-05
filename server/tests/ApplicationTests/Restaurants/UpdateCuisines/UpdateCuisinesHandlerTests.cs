using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateCuisines;
using ApplicationTests.Services.Authentication;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;
using static Web.Error;

namespace ApplicationTests.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateCuisinesHandler handler;

        public UpdateCuisinesHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateCuisinesHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Updates_The_Restaurants_Cuisines()
        {
            var managerId = new UserId(Guid.NewGuid());

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                managerId,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(54.0f, -2.0f)
            );

            var cuisine = new Cuisine("Pizza");

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.CuisineRepositorySpy.Add(cuisine);

            authenticatorSpy.SignIn(managerId);

            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = restaurant.Id.Value,
                Cuisines = new() { "Pizza" },
            };

            var result = await handler.Handle(command, default);

            Assert.True(result.IsSuccess);

            Assert.Single(restaurant.Cuisines);
            Assert.Equal("Pizza", restaurant.Cuisines[0].Name);

            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_Restaurant_Not_Found()
        {
            var cuisine = new Cuisine("Pizza");

            await unitOfWorkSpy.CuisineRepositorySpy.Add(cuisine);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = Guid.NewGuid(),
                Cuisines = new() { "Pizza" },
            };

            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Requires_Authorization()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var cuisine = new Cuisine("Pizza");

            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);
            await unitOfWorkSpy.CuisineRepositorySpy.Add(cuisine);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = restaurant.Id.Value,
            };

            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
