using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Services.Geocoding;
using WebTests.Features.Events;
using WebTests.Services.Geocoding;
using WebTests.Services.Hashing;
using WebTests.Features.Users;
using Web.Domain;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandlerTests
    {
        private readonly HasherFake hasherFake;

        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RestaurantRepositorySpy restaurantRepositorySpy;
        private readonly RestaurantManagerRepositorySpy restaurantManagerRepositorySpy;
        private readonly EventRepositorySpy eventRepositorySpy;

        private readonly GeocoderSpy geocoderSpy;

        private readonly RegisterRestaurantHandler handler;

        public RegisterRestaurantHandlerTests()
        {
            hasherFake = new HasherFake();

            unitOfWorkSpy = new UnitOfWorkSpy();
            restaurantRepositorySpy = unitOfWorkSpy.RestaurantRepositorySpy;
            restaurantManagerRepositorySpy = unitOfWorkSpy.RestaurantManagerRepositorySpy;
            eventRepositorySpy = unitOfWorkSpy.EventRepositorySpy;

            geocoderSpy = new GeocoderSpy();

            handler = new RegisterRestaurantHandler(
                hasherFake,
                unitOfWorkSpy,
                geocoderSpy,
                new ClockStub());
        }

        [Fact]
        public async Task It_Creates_A_New_Restaurant()
        {
            geocoderSpy.Result = Result.Ok(new GeocodingResult
            {
                FormattedAddress = "1 Maine Road, Manchester, MN121NM, UK",
                Coordinates = new Coordinates(1, 1),
            });

            var command = new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "test@email.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var manager = restaurantManagerRepositorySpy.Managers.Single();
            var hashedPassword = hasherFake.Hash(command.ManagerPassword);
            Assert.Equal(command.ManagerName, manager.Name);
            Assert.Equal(command.ManagerEmail, manager.Email.Address);
            Assert.Equal(hashedPassword, manager.Password);

            var restaurant = restaurantRepositorySpy.Restaurants.Single();
            Assert.Equal(manager.Id, restaurant.ManagerId);
            Assert.Equal(command.RestaurantName, restaurant.Name);
            Assert.Equal(command.RestaurantPhoneNumber, restaurant.PhoneNumber.Number);
            Assert.Equal(command.Address, geocoderSpy.SearchAddress);
            Assert.Equal(restaurant.Address.Value, geocoderSpy.Result.Value.FormattedAddress);

            var restaurantRegisteredEvent = (RestaurantRegisteredEvent)eventRepositorySpy
                .Events
                .Single();
            Assert.Equal(restaurant.Id, restaurantRegisteredEvent.RestaurantId);
            Assert.Equal(manager.Id, restaurantRegisteredEvent.ManagerId);

            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Creates_An_Empty_Restaurant_Menu()
        {
            geocoderSpy.Result = Result.Ok(new GeocodingResult
            {
                FormattedAddress = "1 Maine Road, Manchester, MN121NM, UK",
                Coordinates = new Coordinates(1, 1),
            });

            var command = new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "test@email.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK"
            };
            await handler.Handle(command, CancellationToken.None);

            var restaurant = restaurantRepositorySpy.Restaurants.Single();
            var menu = unitOfWorkSpy.MenuRepositorySpy.Menus.First();
            Assert.Equal(menu.RestaurantId, restaurant.Id);
        }
    }
}
