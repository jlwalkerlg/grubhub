using System.Linq;
using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;
using Web.Features.Menus;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemIntegrationTests : WebIntegrationTestBase
    {
        public RemoveMenuItemIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Removes_An_Item_From_The_Menu()
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
            menu.GetCategory("Pizza").AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Delete($"/restaurants/{restaurant.Id.Value}/menu/categories/Pizza/items/Margherita");

            Assert.Equal(204, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            Assert.Empty(menuDto.Categories.Single().Items);
        }
    }
}
