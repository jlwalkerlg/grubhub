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
            var category = new MenuCategory()
            {
                Name = "Pizza",
            };

            var menu = new Menu()
            {
                Categories = new() { category },
            };

            fixture.Insert(menu);

            var request = new RenameMenuCategoryRequest()
            {
                NewName = "Curry",
            };

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Put(
                $"/restaurants/{menu.RestaurantId}/menu/categories/{category.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.MenuCategories.Single());

            found.Name.ShouldBe(request.NewName);
        }
    }
}
