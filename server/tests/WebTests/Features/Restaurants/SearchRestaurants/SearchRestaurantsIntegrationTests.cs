using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Restaurants.SearchRestaurants;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsIntegrationTests : IntegrationTestBase
    {
        public SearchRestaurantsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Restaurants()
        {
            var italian = new Cuisine() { Name = "Italian" };
            var thai = new Cuisine() { Name = "Thai" };
            var greek = new Cuisine() { Name = "Greek" };
            var indian = new Cuisine() { Name = "Indian" };

            var r1 = new Restaurant()
            {
                Latitude = GeocoderStub.Latitude,
                Longitude = GeocoderStub.Longitude - 0.05f,
                MondayOpen = TimeSpan.Zero,
                TuesdayOpen = TimeSpan.Zero,
                WednesdayOpen = TimeSpan.Zero,
                ThursdayOpen = TimeSpan.Zero,
                FridayOpen = TimeSpan.Zero,
                SaturdayOpen = TimeSpan.Zero,
                SundayOpen = TimeSpan.Zero,
                Cuisines = new() { thai },
            };
            r1.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r2 = new Restaurant()
            {
                Latitude = GeocoderStub.Latitude,
                Longitude = GeocoderStub.Longitude,
                MondayOpen = TimeSpan.Zero,
                TuesdayOpen = TimeSpan.Zero,
                WednesdayOpen = TimeSpan.Zero,
                ThursdayOpen = TimeSpan.Zero,
                FridayOpen = TimeSpan.Zero,
                SaturdayOpen = TimeSpan.Zero,
                SundayOpen = TimeSpan.Zero,
                Cuisines = new() { italian, greek },
            };
            r2.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            var r3 = new Restaurant()
            {
                Latitude = GeocoderStub.Latitude,
                Longitude = GeocoderStub.Longitude - 0.1f,
                MondayOpen = TimeSpan.Zero,
                TuesdayOpen = TimeSpan.Zero,
                WednesdayOpen = TimeSpan.Zero,
                ThursdayOpen = TimeSpan.Zero,
                FridayOpen = TimeSpan.Zero,
                SaturdayOpen = TimeSpan.Zero,
                SundayOpen = TimeSpan.Zero,
                Cuisines = new() { indian },
            };
            r3.Menu.Categories.Add(new MenuCategory() { Items = { new MenuItem() } });

            fixture.Insert(r1, r2, r3, italian, thai, greek, indian);

            var response = await fixture.GetClient().Get(
                "/restaurants?postcode=BD181LT&sort_by=distance&cuisines=Thai,Greek");

            var restaurants = await response.GetData<List<RestaurantSearchResult>>();

            restaurants.Count.ShouldBe(2);
            restaurants[0].Id.ShouldBe(r2.Id);
            restaurants[1].Id.ShouldBe(r1.Id);
        }
    }
}
