using System.Linq;
using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Menus.AddMenuCategory;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.AddMenuCategory
{
    public class AddMenuCategoryTests : WebTestBase
    {
        public AddMenuCategoryTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Adds_A_Category_To_The_Menu()
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

            var response = await Post($"/restaurants/{restaurant.Id.Value}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "Pizza",
            });

            Assert.Equal(201, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            Assert.Equal("Pizza", menuDto.Categories.Single().Name);
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "Pizza",
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var categoryName = "Pizza";

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
            menu.AddCategory(categoryName);

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var response = await Post($"/restaurants/{restaurant.Id.Value}/menu/categories", new AddMenuCategoryRequest
            {
                Name = categoryName,
            });

            Assert.NotNull(await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login(Guid.NewGuid());

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "",
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("name"));
        }
    }
}
