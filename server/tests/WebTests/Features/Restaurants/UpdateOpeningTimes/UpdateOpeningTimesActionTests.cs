using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateOpeningTimes;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesActionTests : WebActionTestBase
    {
        public UpdateOpeningTimesActionTests(WebActionTestFixture fixture) : base(fixture)
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
