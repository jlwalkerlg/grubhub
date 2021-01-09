using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemActionTests : HttpTestBase
    {
        public RemoveMenuItemActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().Result.ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
