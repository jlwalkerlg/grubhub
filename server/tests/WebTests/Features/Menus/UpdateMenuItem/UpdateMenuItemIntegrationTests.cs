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

            var request = new UpdateMenuItemRequest()
            {
                Name = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await fixture.GetAuthenticatedClient(menu.Restaurant.ManagerId).Put(
                $"/restaurants/{menu.RestaurantId}/menu/categories/{category.Id}/items/{item.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.MenuItems.Single());

            found.Name.ShouldBe(request.Name);
            found.Description.ShouldBe(request.Description);
            found.Price.ShouldBe(request.Price);
        }
    }
}
