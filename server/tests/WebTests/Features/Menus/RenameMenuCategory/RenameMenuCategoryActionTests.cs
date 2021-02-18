using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.RenameMenuCategory;
using Xunit;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryActionTests : ActionTestBase
    {
        public RenameMenuCategoryActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new RenameMenuCategoryRequest()
            {
                NewName = "Curry",
            };

            var response = await GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new RenameMenuCategoryRequest()
            {
                NewName = "",
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(422);
            response.GetErrors().ShouldContainKey("newName");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new RenameMenuCategoryRequest()
            {
                NewName = "Curry",
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
