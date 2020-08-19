using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.ApplicationTests.Doubles.GeocoderSpy;
using FoodSnap.ApplicationTests.Users;
using Xunit;

namespace FoodSnap.ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RestaurantRepositorySpy restaurantRepositorySpy;
        private readonly RestaurantApplicationRepositorySpy restaurantApplicationRepositorySpy;
        private readonly RestaurantManagerRepositorySpy restaurantManagerRepositorySpy;
        private readonly EventRepositorySpy eventRepositorySpy;
        private readonly GeocoderSpy geocoderSpy;

        private readonly RegisterRestaurantCommand command;
        private readonly RegisterRestaurantHandler handler;

        public RegisterRestaurantHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            restaurantRepositorySpy = unitOfWorkSpy.RestaurantRepositorySpy;
            restaurantApplicationRepositorySpy = unitOfWorkSpy.RestaurantApplicationRepositorySpy;
            restaurantManagerRepositorySpy = unitOfWorkSpy.RestaurantManagerRepositorySpy;
            eventRepositorySpy = unitOfWorkSpy.EventRepositorySpy;
            geocoderSpy = new GeocoderSpy();

            command = new RegisterRestaurantCommandBuilder().Build();
            handler = new RegisterRestaurantHandler(unitOfWorkSpy, geocoderSpy);
        }

        [Fact]
        public async Task It_Creates_A_New_Restaurant()
        {
            geocoderSpy.CoordinatesDto = new CoordinatesDto
            {
                Latitude = 0,
                Longitude = 0
            };

            unitOfWorkSpy.OnCommit = () =>
            {
                var restaurant = restaurantRepositorySpy.Restaurants
                    .Where(x =>
                    {
                        return x.Name == command.RestaurantName
                            && x.PhoneNumber.Number == command.RestaurantPhoneNumber
                            && x.Address.Line1 == command.AddressLine1
                            && x.Address.Line2 == command.AddressLine2
                            && x.Address.Town == command.Town
                            && x.Address.Postcode.Code == command.Postcode
                            && x.Coordinates.Latitude == geocoderSpy.CoordinatesDto.Latitude
                            && x.Coordinates.Longitude == geocoderSpy.CoordinatesDto.Longitude;
                    })
                    .SingleOrDefault();

                var application = restaurantApplicationRepositorySpy.Applications
                    .Where(x => x.RestaurantId == restaurant.Id)
                    .SingleOrDefault();

                var manager = restaurantManagerRepositorySpy.Managers
                    .Where(x =>
                    {
                        return x.Name == command.ManagerName
                            && x.Email.Address == command.ManagerEmail
                            && x.Password == command.ManagerPassword
                            && x.RestaurantId == restaurant.Id;
                    })
                    .SingleOrDefault();

                var restaurantRegisteredEvent = (RestaurantRegisteredEvent)eventRepositorySpy.Events
                    .Where(x => x.GetType() == typeof(RestaurantRegisteredEvent))
                    .SingleOrDefault();

                Assert.NotNull(restaurant);
                Assert.NotNull(application);
                Assert.NotNull(manager);
                Assert.NotNull(restaurantRegisteredEvent);

                Assert.Equal(restaurant.Id, restaurantRegisteredEvent.RestaurantId);
                Assert.Equal(manager.Id, restaurantRegisteredEvent.ManagerId);
            };

            var result = await handler.Handle(command);

            Assert.True(unitOfWorkSpy.Commited);
        }
    }
}
