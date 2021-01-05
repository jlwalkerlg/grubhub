using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateCuisines;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesActionTests : WebActionTestBase
    {
        public UpdateCuisinesActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateCuisinesRequest();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/cuisines", request);

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var request = new UpdateCuisinesRequest();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/cuisines", request);

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
