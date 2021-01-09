using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var item = new MenuItem()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var category = new MenuCategory()
            {
                Name = "Pizza",
                Items = new() { item },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = new() { category },
            };

            fixture.Insert(manager, restaurant, menu);

            var response = await fixture.GetAuthenticatedClient(manager.Id).Delete(
                $"/restaurants/{restaurant.Id}/menu/categories/Pizza/items/Margherita");

            response.StatusCode.ShouldBe(204);

            var found = fixture.UseTestDbContext(db => db.MenuItems.ToList());

            found.ShouldBeEmpty();
        }
    }
}
