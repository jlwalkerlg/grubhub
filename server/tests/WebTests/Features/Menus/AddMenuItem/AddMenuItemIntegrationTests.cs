using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuItem;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.AddMenuItem
{
    public class AddMenuItemIntegrationTests : IntegrationTestBase
    {
        public AddMenuItemIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Adds_An_Item_To_The_Menu()
        {
            var category = new MenuCategory();

            var menu = new Menu()
            {
                Categories = new() { category }
            };

            fixture.Insert(menu);

            var request = new AddMenuItemRequest()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            };

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Post(
                $"/restaurants/{menu.RestaurantId}/menu/categories/{category.Id}/items",
                request);

            response.StatusCode.ShouldBe(201);

            var item = fixture.UseTestDbContext(db => db.MenuItems.Single());

            item.MenuCategoryId.ShouldBe(category.Id);
            item.Name.ShouldBe(request.Name);
            item.Description.ShouldBe(request.Description);
            item.Price.ShouldBe(request.Price);
        }
    }
}
