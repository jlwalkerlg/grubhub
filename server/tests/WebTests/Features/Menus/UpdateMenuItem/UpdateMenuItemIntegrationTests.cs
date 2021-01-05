using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemIntegrationTests : WebIntegrationTestBase
    {
        public UpdateMenuItemIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Renames_An_Item()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.GetCategory("Pizza").AddItem("Margherita", "Cheese & tomato", new Money(10m));

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Put($"/restaurants/{restaurant.Id.Value}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            });

            Assert.Equal(200, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            var categoryDto = menuDto.Categories.Single();
            var itemDto = categoryDto.Items.Single();
            Assert.Equal("Hawaiian", itemDto.Name);
            Assert.Equal("Ham & pineapple", itemDto.Description);
            Assert.Equal(11.99m, itemDto.Price);
        }
    }
}
