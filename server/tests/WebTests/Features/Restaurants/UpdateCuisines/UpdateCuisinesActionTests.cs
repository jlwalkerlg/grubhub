using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateCuisines;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesActionTests : HttpTestBase
    {
        public UpdateCuisinesActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateCuisinesRequest();

            var response = await fixture.GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/cuisines",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateCuisinesRequest();

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.Empty}/cuisines",
                request);

            response.StatusCode.ShouldBe(422);
            response.GetErrors().ShouldContainKey("restaurantId");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateCuisinesRequest();

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/cuisines",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
