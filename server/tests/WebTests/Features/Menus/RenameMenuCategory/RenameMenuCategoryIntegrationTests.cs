using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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

            var request = new RenameMenuCategoryRequest()
            {
                NewName = "Curry",
            };

            var response = await fixture.GetAuthenticatedClient(manager.Id).Put(
                $"/restaurants/{restaurant.Id}/menu/categories/Pizza",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.MenuCategories.Single());

            found.Name.ShouldBe(request.NewName);
        }
    }
}
