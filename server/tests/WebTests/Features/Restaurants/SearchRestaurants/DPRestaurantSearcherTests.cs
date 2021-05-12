using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Domain;
using Web.Features.Restaurants.SearchRestaurants;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.SearchRestaurants
{
    public class DPRestaurantSearcherTests : IntegrationTestBase
    {
        private readonly DateTimeProviderStub dateTimeProvider;
        private readonly DPRestaurantSearcher searcher;

        public DPRestaurantSearcherTests(IntegrationTestFixture fixture) : base(fixture)
        {
            dateTimeProvider = new DateTimeProviderStub();

            searcher = new DPRestaurantSearcher(
                factory.Services.GetRequiredService<IDbConnectionFactory>(),
                dateTimeProvider
            );
        }

        // Restaurants should only appear in search results when they
        // have been approved, are currently open, are within delivery range,
        // have at least one menu item, and have billing enabled.
        [Fact]
        public async Task It_Only_Returns_Suitable_Restaurants()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            // not approved
            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.PendingApproval,
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            // not open
            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = null,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.Approved,
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            // out of range
            var r3 = new Restaurant()
            {
                Latitude = 55.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.Approved,
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            // billing disabled
            var r4 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.Approved,
                BillingAccount = new BillingAccount()
                {
                    Enabled = false,
                },
            };
            r4.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            // no menu items
            var r5 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.Approved,
            };

            var expected = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 5,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Status = Web.Domain.Restaurants.RestaurantStatus.Approved,
            };
            expected.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3, r4, r5, expected);

            var response = await searcher.Search(new Coordinates(54.0f, -2.0f));

            response.Restaurants.ShouldHaveSingleItem();
            response.Restaurants.Single().Id.ShouldBe(expected.Id);
            response.Count.ShouldBe(1);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Distance()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.15f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.1f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3);

            var response = await searcher.Search(
                new Coordinates(54.0f, -2.0f),
                new RestaurantSearchOptions()
                {
                    SortBy = "distance",
                });

            response.Restaurants.Count.ShouldBe(3);
            response.Restaurants[0].Id.ShouldBe(r2.Id);
            response.Restaurants[1].Id.ShouldBe(r3.Id);
            response.Restaurants[2].Id.ShouldBe(r1.Id);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Min_Order()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                MinimumDeliverySpend = 1500,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                MinimumDeliverySpend = 600,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                MinimumDeliverySpend = 1000,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3);

            var response = await searcher.Search(
                new Coordinates(54.0f, -2.0f),
                new RestaurantSearchOptions()
                {
                    SortBy = "min_order",
                });

            response.Restaurants.Count.ShouldBe(3);
            response.Restaurants[0].Id.ShouldBe(r2.Id);
            response.Restaurants[1].Id.ShouldBe(r3.Id);
            response.Restaurants[2].Id.ShouldBe(r1.Id);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Delivery_Fee()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                DeliveryFee = 300,
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                DeliveryFee = 0,
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                DeliveryFee = 150,
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3);

            var response = await searcher.Search(
                new Coordinates(54.0f, -2.0f),
                new RestaurantSearchOptions()
                {
                    SortBy = "delivery_fee",
                });

            response.Restaurants.Count.ShouldBe(3);
            response.Restaurants[0].Id.ShouldBe(r2.Id);
            response.Restaurants[1].Id.ShouldBe(r3.Id);
            response.Restaurants[2].Id.ShouldBe(r1.Id);
        }

        [Fact]
        public async Task It_Sorts_Restaurants_By_Time()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                EstimatedDeliveryTimeInMinutes = 60,
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                EstimatedDeliveryTimeInMinutes = 30,
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                EstimatedDeliveryTimeInMinutes = 40,
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3);

            var response = await searcher.Search(
                new Coordinates(54.0f, -2.0f),
                new RestaurantSearchOptions()
                {
                    SortBy = "time",
                });

            response.Restaurants.Count.ShouldBe(3);
            response.Restaurants[0].Id.ShouldBe(r2.Id);
            response.Restaurants[1].Id.ShouldBe(r3.Id);
            response.Restaurants[2].Id.ShouldBe(r1.Id);
        }

        [Fact]
        public async Task It_Filters_Restaurants_By_Cuisine()
        {
            dateTimeProvider.UtcNow = DateTimeOffset.Parse("Tue, 15 Mar 2005 12:00:00 GMT");

            var thai = new Cuisine() { Name = "Thai" };
            var italian = new Cuisine() { Name = "Italian" };
            var indian = new Cuisine() { Name = "Indian" };

            var r1 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Cuisines = new() { thai, italian },
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Cuisines = new() { thai },
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = 54.0f,
                Longitude = -2.0f,
                MaxDeliveryDistanceInKm = 10,
                TuesdayOpen = TimeSpan.Zero,
                TuesdayClose = null,
                Cuisines = new() { indian },
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            Insert(r1, r2, r3);

            var response = await searcher.Search(
                new Coordinates(54.0f, -2.0f),
                new RestaurantSearchOptions()
                {
                    Cuisines = new() { "Thai", "Italian" }
                });

            response.Restaurants.Count.ShouldBe(2);
            response.Restaurants.ShouldContain(x => x.Id == r1.Id);
            response.Restaurants.ShouldContain(x => x.Id == r2.Id);
        }
    }
}
