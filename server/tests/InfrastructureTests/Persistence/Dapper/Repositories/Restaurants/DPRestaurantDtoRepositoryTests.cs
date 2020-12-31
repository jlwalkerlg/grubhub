using System;
using System.Threading.Tasks;
using Domain;
using Domain.Restaurants;
using Domain.Users;
using Infrastructure.Persistence.Dapper.Repositories.Restaurants;
using Xunit;

namespace InfrastructureTests.Persistence.Dapper.Repositories.Restaurants
{
    public class DPRestaurantDtoRepositoryTests : RepositoryTestBase
    {
        private readonly ClockStub clock;
        private readonly DPRestaurantDtoRepository repository;

        public DPRestaurantDtoRepositoryTests()
        {
            clock = new ClockStub();

            repository = new DPRestaurantDtoRepository(
                new TestDbConnectionFactory(Config.TestDbConnectionString),
                clock
            );
        }

        [Fact]
        public async Task It_Retrieves_Open_Restaurants_Within_Range()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            // not approved
            var r1 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.0f,
                maxDeliveryDistance: 5,
                openingTimes: new OpeningTimes()
                {
                    Tuesday = OpeningHours.Parse("00:00", null),
                },
                approved: false);

            // not open
            var r2 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.0f,
                maxDeliveryDistance: 5,
                openingTimes: new OpeningTimes(),
                approved: true);

            // out of range
            var r3 = await AddRestaurant(
                latitude: 55.0f,
                longitude: -2.0f,
                maxDeliveryDistance: 5,
                openingTimes: new OpeningTimes()
                {
                    Tuesday = OpeningHours.Parse("00:00", null),
                },
                approved: true);

            // expected
            var r4 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.0f,
                maxDeliveryDistance: 5,
                openingTimes: new OpeningTimes()
                {
                    Tuesday = OpeningHours.Parse("00:00", null),
                },
                approved: true);

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f));

            Assert.Single(restaurants);
            r4.AssertEqual(restaurants[0]);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Distance()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.15f);
            var r2 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.0f);
            var r3 = await AddRestaurant(
                latitude: 54.0f,
                longitude: -2.1f);

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "distance",
            });

            Assert.Equal(3, restaurants.Count);
            r2.AssertEqual(restaurants[0]);
            r3.AssertEqual(restaurants[1]);
            r1.AssertEqual(restaurants[2]);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Min_Order()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(minDeliverySpend: 15.00m);
            var r2 = await AddRestaurant(minDeliverySpend: 6.00m);
            var r3 = await AddRestaurant(minDeliverySpend: 10.00m);

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "min_order",
            });

            Assert.Equal(3, restaurants.Count);
            r2.AssertEqual(restaurants[0]);
            r3.AssertEqual(restaurants[1]);
            r1.AssertEqual(restaurants[2]);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Delivery_Fee()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(deliveryFee: 3.00m);
            var r2 = await AddRestaurant(deliveryFee: 0);
            var r3 = await AddRestaurant(deliveryFee: 1.50m);

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "delivery_fee",
            });

            Assert.Equal(3, restaurants.Count);
            r2.AssertEqual(restaurants[0]);
            r3.AssertEqual(restaurants[1]);
            r1.AssertEqual(restaurants[2]);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Time()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(deliveryTime: 60);
            var r2 = await AddRestaurant(deliveryTime: 30);
            var r3 = await AddRestaurant(deliveryTime: 40);

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "time",
            });

            Assert.Equal(3, restaurants.Count);
            r2.AssertEqual(restaurants[0]);
            r3.AssertEqual(restaurants[1]);
            r1.AssertEqual(restaurants[2]);
        }

        private async Task<Restaurant> AddRestaurant(
            float latitude = 54.0f,
            float longitude = -2.0f,
            int maxDeliveryDistance = 10,
            OpeningTimes openingTimes = null,
            bool approved = true,
            decimal minDeliverySpend = 10.00m,
            decimal deliveryFee = 0,
            int deliveryTime = 40)
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                Guid.NewGuid().ToString(),
                new Email(Guid.NewGuid() + "@gmail.com"),
                Guid.NewGuid().ToString()
            );

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                Guid.NewGuid().ToString(),
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(latitude, longitude)
            )
            {
                OpeningTimes = openingTimes,
                MaxDeliveryDistanceInKm = maxDeliveryDistance,
                MinimumDeliverySpend = new Money(minDeliverySpend),
                DeliveryFee = new Money(deliveryFee),
                EstimatedDeliveryTimeInMinutes = deliveryTime,
            };

            if (openingTimes == null)
            {
                restaurant.OpeningTimes = new OpeningTimes()
                {
                    Monday = OpeningHours.Parse("00:00", null),
                    Tuesday = OpeningHours.Parse("00:00", null),
                    Wednesday = OpeningHours.Parse("00:00", null),
                    Thursday = OpeningHours.Parse("00:00", null),
                    Friday = OpeningHours.Parse("00:00", null),
                    Saturday = OpeningHours.Parse("00:00", null),
                    Sunday = OpeningHours.Parse("00:00", null),
                };
            }

            if (approved)
            {
                restaurant.Approve();
            }

            await context.AddRangeAsync(manager, restaurant);
            await context.SaveChangesAsync();

            return restaurant;
        }
    }
}
