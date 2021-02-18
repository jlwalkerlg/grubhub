using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryIntegrationTests : IntegrationTestBase
    {
        public RemoveMenuCategoryIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Soft_Deletes_The_Category()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();

            menu.Categories.Add(category);

            Insert(restaurant);

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Delete(
                $"/restaurants/{restaurant.Id}/menu/categories/{category.Id}");

            response.StatusCode.ShouldBe(204);

            var categories = UseTestDbContext(db => db.MenuCategories.ToArray());

            categories.ShouldHaveSingleItem();

            var found = categories.Single();

            found.IsDeleted.ShouldBeTrue();
        }
    }
}
