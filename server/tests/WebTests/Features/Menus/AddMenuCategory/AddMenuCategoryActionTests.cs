using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryActionTests : HttpTestBase
    {
        public AddMenuCategoryActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new AddMenuCategoryRequest();

            var response = await fixture.GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new AddMenuCategoryRequest()
            {
                Name = "",
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories",
                request);

            response.StatusCode.ShouldBe(422);
            response.GetErrors().ShouldContainKey("name");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new AddMenuCategoryRequest()
            {
                Name = "Pizza"
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
