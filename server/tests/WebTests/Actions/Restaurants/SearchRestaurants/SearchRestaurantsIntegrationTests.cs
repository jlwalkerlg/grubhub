using System;
using System.Collections.Generic;
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
        public async Task It_Returns_Sorted_Restaurants()
        {
            var r1 = await AddRestaurant(54.0f, -2.15f);
            var r2 = await AddRestaurant(54.0f, -2.0f);
            var r3 = await AddRestaurant(54.0f, -2.1f);

            var restaurants = await Get<List<RestaurantDto>>("/restaurants?postcode=BD181LT&sort_by=distance");

            Assert.Equal(3, restaurants.Count);
            r2.AssertEqual(restaurants[0]);
            r3.AssertEqual(restaurants[1]);
            r1.AssertEqual(restaurants[2]);
        }

        private async Task<Restaurant> AddRestaurant(float latitude, float longitude)
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

            await fixture.InsertDb(manager, restaurant);

            return restaurant;
        }
    }
}
