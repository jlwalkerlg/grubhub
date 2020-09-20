using System.Threading;
using System;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.AcceptRestaurantRegistration;
using Xunit;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain;
using System.Linq;
using FoodSnap.Application.Restaurants;

namespace FoodSnap.ApplicationTests.Restaurants.AcceptRestaurantRegistration
{
    public class AcceptRestaurantRegistrationHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AcceptRestaurantRegistrationHandler handler;

        public AcceptRestaurantRegistrationHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new AcceptRestaurantRegistrationHandler(unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Accepts_The_Restaurant()
        {
            var restaurant = new Restaurant(
                Guid.NewGuid(),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "Manchester Road",
                    "",
                    "Manchester",
                    new Postcode("MN121NM")),
                new Coordinates(0, 0));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new AcceptRestaurantRegistrationCommand
            {
                RestaurantId = restaurant.Id
            };
            var result = await handler.Handle(command, default(CancellationToken));

            Assert.True(result.IsSuccess);
            Assert.Equal(RestaurantApplicationStatus.Accepted, restaurant.Status);
            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Creates_An_Empty_Restaurant_Menu()
        {
            var restaurant = new Restaurant(
                Guid.NewGuid(),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "Manchester Road",
                    "",
                    "Manchester",
                    new Postcode("MN121NM")),
                new Coordinates(0, 0));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new AcceptRestaurantRegistrationCommand
            {
                RestaurantId = restaurant.Id
            };
            await handler.Handle(command, default(CancellationToken));

            var menu = unitOfWorkSpy.MenuRepositorySpy.Menus.First();
            Assert.Equal(menu.RestaurantId, restaurant.Id);
        }

        [Fact]
        public async Task It_Creates_A_Restaurant_Application_Accepted_Event()
        {
            var restaurant = new Restaurant(
                Guid.NewGuid(),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "Manchester Road",
                    "",
                    "Manchester",
                    new Postcode("MN121NM")),
                new Coordinates(0, 0));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new AcceptRestaurantRegistrationCommand
            {
                RestaurantId = restaurant.Id
            };
            var result = await handler.Handle(command, default(CancellationToken));

            var @event = (await unitOfWorkSpy.EventRepositorySpy.All())
                .OfType<RestaurantAcceptedEvent>()
                .Single();

            Assert.Equal(@event.RestaurantId, restaurant.Id);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_The_Restaurant_Was_Not_Found()
        {
            var command = new AcceptRestaurantRegistrationCommand
            {
                RestaurantId = Guid.NewGuid()
            };
            var result = await handler.Handle(command, default(CancellationToken));

            Assert.False(result.IsSuccess);
        }
    }
}
