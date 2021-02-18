using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemActionTests : ActionTestBase
    {
        public UpdateMenuItemActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateMenuItemRequest()
            {
                Name = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateMenuItemRequest()
            {
                Name = "",
                Description = new string('c', 281),
                Price = -1m,
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("name");
            errors.ShouldContainKey("description");
            errors.ShouldContainKey("price");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateMenuItemRequest()
            {
                Name = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
