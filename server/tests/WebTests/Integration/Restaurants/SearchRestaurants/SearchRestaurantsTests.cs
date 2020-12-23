using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Integration.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsTests : WebIntegrationTestBase
    {
        public SearchRestaurantsTests(WebAppIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Open_Approved_Restaurants_Only()
        {
            var now = ClockStub.Now;

            // not open
            var r1 = await InsertRestaurant(54, -2, 10, new OpeningTimes(), true);

            // not approved
            var r2 = await InsertRestaurant(54, -2, 10, OpeningTimes.FromDays(new()
            {
                { now.DayOfWeek, new OpeningHours(TimeSpan.Zero, TimeSpan.FromHours(24)) }
            }), false);

            // open and approved
            var r3 = await InsertRestaurant(54, -2, 10, OpeningTimes.FromDays(new()
            {
                { now.DayOfWeek, new OpeningHours(TimeSpan.Zero, TimeSpan.FromHours(24)) }
            }), true);

            var restaurants = await Get<List<RestaurantDto>>("/restaurants?postcode=MN121NM");

            Assert.Single(restaurants);
            Assert.Equal(r3.Id.Value, restaurants[0].Id);
        }

        [Fact]
        public async Task It_Returns_Restaurants_Within_Max_Delivery_Distance_Only()
        {
            var now = ClockStub.Now;

            // 54 lat, -2.1 lng is 6.54 km away from 54 lat, -2 lng
            var r1 = await InsertRestaurant(54, -2.1f, 5, OpeningTimes.FromDays(new()
            {
                { now.DayOfWeek, new OpeningHours(TimeSpan.Zero, TimeSpan.FromHours(24)) },
            }), true);
            var r2 = await InsertRestaurant(54, -2.1f, 10, OpeningTimes.FromDays(new()
            {
                { now.DayOfWeek, new OpeningHours(TimeSpan.Zero, TimeSpan.FromHours(24)) },
            }), true);

            var restaurants = await Get<List<RestaurantDto>>("/restaurants?postcode=MN121NM");

            Assert.Single(restaurants);
            Assert.Equal(r2.Id.Value, restaurants[0].Id);
        }

        private async Task<Restaurant> InsertRestaurant(
            float lat,
            float lng,
            int maxDeliveryDistanceInKm,
            OpeningTimes openingTimes,
            bool approved)
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email($"{Guid.NewGuid()}@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(lat, lng));
            restaurant.OpeningTimes = openingTimes;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.DeliveryFee = new Money(1.5m);
            restaurant.MaxDeliveryDistanceInKm = maxDeliveryDistanceInKm;
            restaurant.EstimatedDeliveryTimeInMinutes = 40;

            if (approved)
            {
                restaurant.Approve();
            }

            var menu = new Menu(restaurant.Id);

            await fixture.InsertDb(manager, restaurant, menu);

            return restaurant;
        }
    }
}
