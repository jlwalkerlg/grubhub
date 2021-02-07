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
            var restaurant = new Restaurant();

            fixture.Insert(restaurant);

            var request = new AddMenuCategoryRequest()
            {
                Name = "Pizza",
            };

            var response = await fixture.GetAuthenticatedClient(restaurant.ManagerId).Post(
                $"/restaurants/{restaurant.Id}/menu/categories",
                request);

            response.StatusCode.ShouldBe(201);

            var menu = fixture.UseTestDbContext(db => db.Menus.Single());

            menu.RestaurantId.ShouldBe(restaurant.Id);

            var category = fixture.UseTestDbContext(db => db.MenuCategories.Single());

            category.MenuId.ShouldBe(menu.Id);
            category.Name.ShouldBe(request.Name);
        }
    }
}
