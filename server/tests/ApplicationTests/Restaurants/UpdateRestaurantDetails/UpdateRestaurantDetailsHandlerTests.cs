using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using ApplicationTests.Services.Authentication;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;
using static Web.Error;

namespace ApplicationTests.Restaurants.UpdateRestaurantDetails
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

            authenticatorSpy.SignIn(manager);

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
                RestaurantId = restaurant.Id.Value,
                Name = "Kung Flu",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.PhoneNumber, restaurant.PhoneNumber.Number);
            Assert.Equal(command.MinimumDeliverySpend, restaurant.MinimumDeliverySpend.Amount);
            Assert.Equal(command.DeliveryFee, restaurant.DeliveryFee.Amount);
            Assert.Equal(command.MaxDeliveryDistanceInKm, restaurant.MaxDeliveryDistanceInKm);
            Assert.Equal(command.EstimatedDeliveryTimeInMinutes, restaurant.EstimatedDeliveryTimeInMinutes);

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

            authenticatorSpy.SignIn(manager);

            var command = new UpdateRestaurantDetailsCommand
            {
                RestaurantId = Guid.NewGuid(),
                Name = "Kung Flu",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
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
                RestaurantId = restaurant.Id.Value,
                Name = "Kung Flu",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }
    }
}
