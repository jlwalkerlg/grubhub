using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
            var user = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = user.Id,
            };

            var menu = new Menu()
            {
                Id = 1,
                RestaurantId = restaurant.Id,
            };

            fixture.Insert(user, restaurant, menu);

            var request = new AddMenuCategoryRequest()
            {
                Name = "Pizza",
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Post(
                $"/restaurants/{restaurant.Id}/menu/categories",
                request);

            response.StatusCode.ShouldBe(201);

            var found = fixture.UseTestDbContext(db => db.MenuCategories.Single());

            found.MenuId.ShouldBe(menu.Id);
            found.Name.ShouldBe(request.Name);
        }
    }
}
