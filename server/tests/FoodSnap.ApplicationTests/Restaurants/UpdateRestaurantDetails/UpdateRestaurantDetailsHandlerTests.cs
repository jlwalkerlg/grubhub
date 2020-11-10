using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.UpdateRestaurantDetails;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Shared.Error;

namespace FoodSnap.ApplicationTests.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateRestaurantDetailsHandler handler;

        public UpdateRestaurantDetailsHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateRestaurantDetailsHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Updates_The_Restaurants_Details()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = manager;

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = restaurant.Id.Value,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.PhoneNumber, restaurant.PhoneNumber.Number);

            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Returns_Not_Found_Error_If_Restaurant_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = manager;

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = Guid.NewGuid(),
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Authorises_The_User()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = restaurant.Id.Value,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
