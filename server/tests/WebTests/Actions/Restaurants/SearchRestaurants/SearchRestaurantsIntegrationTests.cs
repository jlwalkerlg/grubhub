using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain;
using Domain.Restaurants;
using Domain.Users;
using SharedTests;
using Xunit;

namespace WebTests.Actions.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsIntegrationTests : WebIntegrationTestBase
    {
        public SearchRestaurantsIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Restaurants()
        {
            var r1 = await AddRestaurant(54.0f, -2.15f, new() { "Thai" });
            var r2 = await AddRestaurant(54.0f, -2.0f, new() { "Italian", "Greek" });
            var r3 = await AddRestaurant(54.0f, -2.1f, new() { "Indian" });

            var restaurants = await Get<List<RestaurantDto>>("/restaurants?postcode=BD181LT&sort_by=distance&cuisines=Thai,Greek");

            Assert.Equal(2, restaurants.Count);
            Assert.True(r2.IsEqual(restaurants[0]));
            Assert.True(r1.IsEqual(restaurants[1]));
        }

        private async Task<Restaurant> AddRestaurant(
            float latitude,
            float longitude,
            List<string> cuisines)
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
                OpeningTimes = new OpeningTimes()
                {
                    Monday = OpeningHours.Parse("00:00", null),
                    Tuesday = OpeningHours.Parse("00:00", null),
                    Wednesday = OpeningHours.Parse("00:00", null),
                    Thursday = OpeningHours.Parse("00:00", null),
                    Friday = OpeningHours.Parse("00:00", null),
                    Saturday = OpeningHours.Parse("00:00", null),
                    Sunday = OpeningHours.Parse("00:00", null),
                },
                MaxDeliveryDistanceInKm = 10,
                MinimumDeliverySpend = new Money(0),
                DeliveryFee = new Money(1.50m),
                EstimatedDeliveryTimeInMinutes = 40,
            };

            restaurant.Approve();

            restaurant.SetCuisines(cuisines.Select(x => new Cuisine(x)));

            await fixture.InsertDb(restaurant, manager);

            return restaurant;
        }
    }
}
