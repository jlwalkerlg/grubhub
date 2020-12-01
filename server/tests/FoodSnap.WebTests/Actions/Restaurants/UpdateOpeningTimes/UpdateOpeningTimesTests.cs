using System;
using System.Threading.Tasks;
using FoodSnap.Web.Actions.Restaurants.UpdateOpeningTimes;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesTests : WebActionTestBase
    {
        public UpdateOpeningTimesTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateOpeningTimesRequest();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/opening-times", request);

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var request = new UpdateOpeningTimesRequest();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/opening-times", request);

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var request = new UpdateOpeningTimesRequest()
            {
                MondayOpen = "45:00",
            };

            var response = await Put($"/restaurants/{Guid.NewGuid()}/opening-times", request);

            Assert.Equal(422, (int)response.StatusCode);

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("mondayOpen"));
        }
    }
}
