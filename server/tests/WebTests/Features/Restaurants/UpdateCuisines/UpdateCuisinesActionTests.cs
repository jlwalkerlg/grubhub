using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateCuisines;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesActionTests : ActionTestBase
    {
        public UpdateCuisinesActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateCuisinesRequest();

            var response = await GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/cuisines",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateCuisinesRequest();

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.Empty}/cuisines",
                request);

            response.StatusCode.ShouldBe(422);
            response.GetErrors().ShouldContainKey("restaurantId");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateCuisinesRequest();

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/cuisines",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
