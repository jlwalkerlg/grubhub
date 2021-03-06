using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemIntegrationTests : IntegrationTestBase
    {
        public RemoveMenuItemIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Soft_Deletes_The_Item()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var item = new MenuItem();

            category.Items.Add(item);
            menu.Categories.Add(category);

            Insert(restaurant);

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Delete(
                $"/restaurants/{restaurant.Id}/menu/categories/{category.Id}/items/{item.Id}");

            response.StatusCode.ShouldBe(204);

            var items = UseTestDbContext(db => db.MenuItems.ToArray());

            items.ShouldBeEmpty();
        }
    }
}
