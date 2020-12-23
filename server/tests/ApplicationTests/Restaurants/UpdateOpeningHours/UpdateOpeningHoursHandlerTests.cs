using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Restaurants.UpdateOpeningHours;
using ApplicationTests.Services.Authentication;
using Domain;
using Domain.Restaurants;
using Domain.Users;
using Xunit;
using static Application.Error;

namespace ApplicationTests.Restaurants.UpdateOpeningHours
{
    public class UpdateOpeningHoursHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;

        public AuthenticatorSpy authenticatorSpy { get; }

        private readonly UpdateOpeningHoursHandler handler;

        public UpdateOpeningHoursHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateOpeningHoursHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Updates_The_Opening_Times()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            authenticatorSpy.SignIn(manager);

            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = restaurant.Id.Value,
                MondayOpen = "16:00",
                MondayClose = "23:30",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.True(unitOfWorkSpy.Commited);

            Assert.NotNull(restaurant.OpeningTimes);
            Assert.NotNull(restaurant.OpeningTimes.Monday);
            Assert.Null(restaurant.OpeningTimes.Tuesday);
            Assert.Null(restaurant.OpeningTimes.Wednesday);
            Assert.Null(restaurant.OpeningTimes.Thursday);
            Assert.Null(restaurant.OpeningTimes.Friday);
            Assert.Null(restaurant.OpeningTimes.Saturday);
            Assert.Null(restaurant.OpeningTimes.Sunday);
            Assert.Equal(16, restaurant.OpeningTimes.Monday.Open.Hours);
            Assert.Equal(0, restaurant.OpeningTimes.Monday.Open.Minutes);
            Assert.Equal(23, restaurant.OpeningTimes.Monday.Close.Hours);
            Assert.Equal(30, restaurant.OpeningTimes.Monday.Close.Minutes);
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = restaurant.Id.Value,
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthorised, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Restaurant_Not_Found()
        {
            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}
