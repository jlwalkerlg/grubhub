using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.ApplicationTests.Doubles.GeocoderSpy;
using FoodSnap.ApplicationTests.Events;
using FoodSnap.ApplicationTests.Users;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
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

            handler = new RegisterRestaurantHandler(hasherFake, unitOfWorkSpy, geocoderSpy);
        }

        [Fact]
        public async Task It_Creates_A_New_Restaurant()
        {
            var command = new RegisterRestaurantCommandBuilder().Build();

            geocoderSpy.Coordinates = new Coordinates(0, 0);

            unitOfWorkSpy.OnCommit = () =>
            {
                var restaurant = restaurantRepositorySpy.Restaurants
                    .Where(x => x.Name == command.RestaurantName)
                    .Single();

                var manager = restaurantManagerRepositorySpy.Managers
                    .Where(x => x.Name == command.ManagerName)
                    .Single();

                var restaurantRegisteredEvent = (RestaurantRegisteredEvent)eventRepositorySpy
                    .Events
                    .OfType<RestaurantRegisteredEvent>()
                    .Single();

                Assert.Equal(command.ManagerName, manager.Name);
                Assert.Equal(command.ManagerEmail, manager.Email.Address);
                Assert.Equal(hasherFake.Hash(command.ManagerPassword), manager.Password);

                Assert.Equal(command.RestaurantName, restaurant.Name);
                Assert.Equal(command.RestaurantPhoneNumber, restaurant.PhoneNumber.Number);
                Assert.Equal(command.AddressLine1, restaurant.Address.Line1);
                Assert.Equal(command.AddressLine2, restaurant.Address.Line2);
                Assert.Equal(command.Town, restaurant.Address.Town);
                Assert.Equal(command.Postcode, restaurant.Address.Postcode.Code);

                Assert.Equal(manager.Id, restaurant.ManagerId);

                Assert.Equal(restaurant.Id, restaurantRegisteredEvent.RestaurantId);
                Assert.Equal(manager.Id, restaurantRegisteredEvent.ManagerId);

                Assert.Equal(restaurant.Address, geocoderSpy.Address);
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(unitOfWorkSpy.Commited);
        }
    }
}
