using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuCategory;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryIntegrationTests : IntegrationTestBase
    {
        public AddMenuCategoryIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Adds_A_Category_To_The_Menu()
        {
            var menu = new Menu()
            {
                Id = 1,
            };

            fixture.Insert(menu);

            var request = new AddMenuCategoryRequest()
            {
                Name = "Pizza",
            };

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Post(
                $"/restaurants/{menu.RestaurantId}/menu/categories",
                request);

            response.StatusCode.ShouldBe(201);

            var category = fixture.UseTestDbContext(db => db.MenuCategories.Single());

            category.MenuId.ShouldBe(menu.Id);
            category.Name.ShouldBe(request.Name);
        }
    }
}
