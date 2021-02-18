using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryActionTests : ActionTestBase
    {
        public AddMenuCategoryActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new AddMenuCategoryRequest();

            var response = await GetClient().Post(
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

            var response = await GetAuthenticatedClient().Post(
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

            var response = await GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
