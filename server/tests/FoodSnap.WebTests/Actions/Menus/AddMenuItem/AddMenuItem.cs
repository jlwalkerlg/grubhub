using System.Linq;
using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Menus.AddMenuItem;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.AddMenuItem
{
    public class AddMenuItemTests : WebTestBase
    {
        public AddMenuItemTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Adds_An_Item_To_The_Menu()
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

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Post($"/restaurants/{restaurant.Id.Value}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            });

            Assert.Equal(201, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            var itemDto = menuDto.Categories.Single().Items.Single();
            Assert.Equal("Margherita", itemDto.Name);
            Assert.Equal("Cheese & tomato", itemDto.Description);
            Assert.Equal(10m, itemDto.Price);
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var itemName = "Margherita";

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
            menu.GetCategory("Pizza").AddItem(itemName, "Cheese & tomato", new Money(10m));

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Post($"/restaurants/{restaurant.Id.Value}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = itemName,
                Description = "Cheese & tomato",
                Price = 10m,
            });

            Assert.NotNull(await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login(Guid.NewGuid());

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "",
                ItemName = "",
                Description = "",
                Price = -10m,
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("categoryName"));
            Assert.True(errors.ContainsKey("itemName"));
            Assert.True(errors.ContainsKey("description"));
            Assert.True(errors.ContainsKey("price"));
        }
    }
}
