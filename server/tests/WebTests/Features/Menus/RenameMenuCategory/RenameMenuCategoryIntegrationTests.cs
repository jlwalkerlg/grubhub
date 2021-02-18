using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus.RenameMenuCategory;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryIntegrationTests : IntegrationTestBase
    {
        public RenameMenuCategoryIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Renames_A_Category()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();

            menu.Categories.Add(category);

            Insert(restaurant);

            var request = new RenameMenuCategoryRequest()
            {
                NewName = "Curry",
            };

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Put(
                $"/restaurants/{restaurant.Id}/menu/categories/{category.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = UseTestDbContext(db => db.MenuCategories.Single());

            found.Name.ShouldBe(request.NewName);
        }
    }
}
