using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Restaurants.UpdateOpeningTimes;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesActionTests : HttpTestBase
    {
        public UpdateOpeningTimesActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateOpeningTimesRequest();

            var response = await fixture.GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/opening-times",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateOpeningTimesRequest()
            {
                MondayOpen = "45:00",
            };

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/opening-times",
                request);

            response.StatusCode.ShouldBe(422);
            response.GetErrors().ShouldContainKey("mondayOpen");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateOpeningTimesRequest();

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/opening-times",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
