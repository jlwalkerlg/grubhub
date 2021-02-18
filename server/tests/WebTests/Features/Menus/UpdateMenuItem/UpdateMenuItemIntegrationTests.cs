using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus.UpdateMenuItem;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemIntegrationTests : IntegrationTestBase
    {
        public UpdateMenuItemIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_A_Menu_Item()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var item = new MenuItem();

            category.Items.Add(item);
            menu.Categories.Add(category);

            Insert(restaurant);

            var request = new UpdateMenuItemRequest()
            {
                Name = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Put(
                $"/restaurants/{restaurant.Id}/menu/categories/{category.Id}/items/{item.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = UseTestDbContext(db => db.MenuItems.Single());

            found.Name.ShouldBe(request.Name);
            found.Description.ShouldBe(request.Description);
            found.Price.ShouldBe((int)(request.Price * 100));
        }
    }
}
