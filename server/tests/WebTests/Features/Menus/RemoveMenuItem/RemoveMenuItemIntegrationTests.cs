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
        public async Task It_Removes_An_Item_From_The_Menu()
        {
            var item = new MenuItem();

            var category = new MenuCategory()
            {
                Items = new() { item },
            };

            var menu = new Menu()
            {
                Categories = new() { category },
            };

            fixture.Insert(menu);

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Delete(
                $"/restaurants/{menu.RestaurantId}/menu/categories/{category.Id}/items/{item.Id}");

            response.StatusCode.ShouldBe(204);

            var found = fixture.UseTestDbContext(db => db.MenuItems.ToList());

            found.ShouldBeEmpty();
        }
    }
}
