using System.Linq;
using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Menus.UpdateMenuItem;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.UpdateMenuItem
{
    public class UpdateMenuItemTests : WebTestBase
    {
        public UpdateMenuItemTests(TestWebApplicationFixture fixture) : base(fixture)
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

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
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

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Put($"/restaurants/{restaurant.Id.Value}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            });

            Assert.NotNull(await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login(Guid.NewGuid());

            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "",
                Description = "",
                Price = -10m,
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("newItemName"));
            Assert.True(errors.ContainsKey("description"));
            Assert.True(errors.ContainsKey("price"));
        }
    }
}
