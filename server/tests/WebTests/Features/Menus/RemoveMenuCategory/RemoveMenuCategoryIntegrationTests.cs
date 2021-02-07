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
        public async Task It_Removes_A_Category_From_The_Menu()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();

            menu.Categories.Add(category);

            fixture.Insert(restaurant);

            var response = await fixture.GetAuthenticatedClient(restaurant.ManagerId).Delete(
                $"/restaurants/{restaurant.Id}/menu/categories/{category.Id}");

            response.StatusCode.ShouldBe(204);

            var categories = fixture.UseTestDbContext(db => db.MenuCategories.ToArray());

            categories.ShouldBeEmpty();
        }
    }
}
