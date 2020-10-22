using System.Linq;
using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Menus.RenameMenuCategory;
using Xunit;

namespace FoodSnap.WebTests.Integration.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryTests : WebTestBase
    {
        public RenameMenuCategoryTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Renames_A_Category()
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

            var response = await Put($"/restaurants/{restaurant.Id.Value}/menu/categories/Pizza", new RenameMenuCategoryRequest
            {
                NewName = "Curry",
            });

            Assert.Equal(200, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            var categoryDto = menuDto.Categories.Single();
            Assert.Equal("Curry", categoryDto.Name);
        }
    }
}