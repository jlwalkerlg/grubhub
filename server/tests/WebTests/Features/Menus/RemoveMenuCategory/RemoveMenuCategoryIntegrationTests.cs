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
            var category = new MenuCategory();

            var menu = new Menu()
            {
                Categories = new() { category },
            };

            fixture.Insert(menu);

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Delete(
                $"/restaurants/{menu.RestaurantId}/menu/categories/{category.Id}");

            response.StatusCode.ShouldBe(204);

            var categories = fixture.UseTestDbContext(db => db.MenuCategories.ToArray());

            categories.ShouldBeEmpty();
        }
    }
}
