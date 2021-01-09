using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var category = new MenuCategory()
            {
                Name = "Pizza",
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = new() { category },
            };

            fixture.Insert(manager, restaurant, menu);

            var response = await fixture.GetAuthenticatedClient(manager.Id).Delete(
                $"/restaurants/{restaurant.Id}/menu/categories/Pizza");

            response.StatusCode.ShouldBe(204);

            var found = fixture.UseTestDbContext(db => db.MenuCategories.ToList());

            found.ShouldBeEmpty();
        }
    }
}
