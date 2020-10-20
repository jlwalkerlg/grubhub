using System.Linq;
using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.RemoveMenuItem
{
    public class RemoveMenuItemTests : WebTestBase
    {
        public RemoveMenuItemTests(TestWebApplicationFixture fixture) : base(fixture)
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

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login(Guid.NewGuid());

            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            Assert.Equal(404, (int)response.StatusCode);
            Assert.NotNull(await response.GetErrorMessage());
        }
    }
}
