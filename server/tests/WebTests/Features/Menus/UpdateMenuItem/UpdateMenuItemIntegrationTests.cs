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
        public async Task It_Renames_A_Menu_Item()
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

            var request = new UpdateMenuItemRequest()
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await fixture.GetAuthenticatedClient(manager.Id).Put(
                $"/restaurants/{restaurant.Id}/menu/categories/Pizza/items/Margherita",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.MenuItems.Single());

            found.Name.ShouldBe(request.NewItemName);
            found.Description.ShouldBe(request.Description);
            found.Price.ShouldBe(request.Price);
        }
    }
}
