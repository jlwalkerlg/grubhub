using System;
using System.Threading.Tasks;
using Web.Data.Dapper.Repositories.Restaurants;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Data.Dapper.Repositories.Restaurants
{
    public class DPRestaurantDtoRepositoryTests : RepositoryTestBase
    {
        private readonly ClockStub clock;
        private readonly DPRestaurantDtoRepository repository;

        public DPRestaurantDtoRepositoryTests()
        {
            clock = new ClockStub();

            repository = new DPRestaurantDtoRepository(
                new TestDbConnectionFactory(TestConfig.InfrastructureTestDbConnectionString),
                clock
            );
        }

        [Fact]
        public async Task It_Gets_A_Restaurant_By_Id()
        {
            var restaurant = await AddRestaurant();

            var cuisine = new Cuisine("Pizza");

            await context.AddAsync(cuisine);
            await context.SaveChangesAsync();

            var found = await repository.GetById(restaurant.Id.Value);
            Assert.True(restaurant.IsEqual(found));
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

            var cuisine = new Cuisine("Pizza");
            r4.SetCuisines(cuisine);

            await context.AddAsync(cuisine);
            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f));

            Assert.Single(restaurants);
            Assert.True(r4.IsEqual(restaurants[0]));
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

            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "distance",
            });

            Assert.Equal(3, restaurants.Count);
            Assert.True(r2.IsEqual(restaurants[0]));
            Assert.True(r3.IsEqual(restaurants[1]));
            Assert.True(r1.IsEqual(restaurants[2]));
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Min_Order()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(minDeliverySpend: 15.00m);
            var r2 = await AddRestaurant(minDeliverySpend: 6.00m);
            var r3 = await AddRestaurant(minDeliverySpend: 10.00m);

            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "min_order",
            });

            Assert.Equal(3, restaurants.Count);
            Assert.True(r2.IsEqual(restaurants[0]));
            Assert.True(r3.IsEqual(restaurants[1]));
            Assert.True(r1.IsEqual(restaurants[2]));
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Delivery_Fee()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(deliveryFee: 3.00m);
            var r2 = await AddRestaurant(deliveryFee: 0);
            var r3 = await AddRestaurant(deliveryFee: 1.50m);

            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "delivery_fee",
            });

            Assert.Equal(3, restaurants.Count);
            Assert.True(r2.IsEqual(restaurants[0]));
            Assert.True(r3.IsEqual(restaurants[1]));
            Assert.True(r1.IsEqual(restaurants[2]));
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Time()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant(deliveryTime: 60);
            var r2 = await AddRestaurant(deliveryTime: 30);
            var r3 = await AddRestaurant(deliveryTime: 40);

            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                SortBy = "time",
            });

            Assert.Equal(3, restaurants.Count);
            Assert.True(r2.IsEqual(restaurants[0]));
            Assert.True(r3.IsEqual(restaurants[1]));
            Assert.True(r1.IsEqual(restaurants[2]));
        }

        [Fact]
        public async Task It_Filters_Restaurants_By_Cuisine()
        {
            clock.UtcNow = DateTime.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = await AddRestaurant();
            var r2 = await AddRestaurant();
            var r3 = await AddRestaurant();

            var thai = new Cuisine("Thai");
            var italian = new Cuisine("Italian");
            var indian = new Cuisine("Indian");

            r1.SetCuisines(thai, italian);
            r2.SetCuisines(thai);
            r3.SetCuisines(indian);

            await context.SaveChangesAsync();

            var restaurants = await repository.Search(new Coordinates(54.0f, -2.0f), new()
            {
                Cuisines = new() { "Thai", "Italian" }
            });

            Assert.Equal(2, restaurants.Count);
            Assert.Single(restaurants, dto => r1.IsEqual(dto));
            Assert.Single(restaurants, dto => r2.IsEqual(dto));
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

            return restaurant;
        }
    }
}
