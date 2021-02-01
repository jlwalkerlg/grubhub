using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateOpeningTimes;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesIntegrationTests : IntegrationTestBase
    {
        public UpdateOpeningTimesIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Opening_Times()
        {
            var restaurant = new Restaurant();

            fixture.Insert(restaurant);

            var request = new UpdateOpeningTimesRequest()
            {
                MondayOpen = "10:30",
                MondayClose = "16:00",
            };

            var response = await fixture.GetAuthenticatedClient(restaurant.ManagerId).Put(
                $"/restaurants/{restaurant.Id}/opening-times",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.Restaurants.Single());

            found.MondayOpen.Value.ShouldBe(TimeSpan.Parse(request.MondayOpen));
            found.MondayClose.Value.ShouldBe(TimeSpan.Parse(request.MondayClose));
            found.TuesdayOpen.ShouldBeNull();
            found.TuesdayClose.ShouldBeNull();
            found.WednesdayOpen.ShouldBeNull();
            found.WednesdayClose.ShouldBeNull();
            found.ThursdayOpen.ShouldBeNull();
            found.ThursdayClose.ShouldBeNull();
            found.FridayOpen.ShouldBeNull();
            found.FridayClose.ShouldBeNull();
            found.SaturdayOpen.ShouldBeNull();
            found.SaturdayClose.ShouldBeNull();
            found.SundayOpen.ShouldBeNull();
            found.SundayClose.ShouldBeNull();
        }
    }
}
