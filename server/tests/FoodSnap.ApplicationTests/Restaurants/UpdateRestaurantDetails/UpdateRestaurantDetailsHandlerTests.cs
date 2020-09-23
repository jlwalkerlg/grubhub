using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.UpdateRestaurantDetails;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;
using static FoodSnap.Application.Error;

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
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = manager;

            var restaurant = new Restaurant(
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Missa Few",
                    "",
                    "NinetyNine",
                    new Postcode("ON33NO")),
                new Coordinates(1, 2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = restaurant.Id,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.True(result.IsSuccess);

            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.PhoneNumber, restaurant.PhoneNumber.Number);

            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Returns_Not_Found_Error_If_Restaurant_Not_Found()
        {
            var manager = new RestaurantManager(
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticatorSpy.User = manager;

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = manager.Id,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }

        [Fact]
        public async Task It_Authorises_The_User()
        {
            var restaurant = new Restaurant(
                Guid.NewGuid(),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Missa Few",
                    "",
                    "NinetyNine",
                    new Postcode("ON33NO")),
                new Coordinates(1, 2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = restaurant.Id,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
