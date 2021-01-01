using System;
using System.Threading.Tasks;
using Application.Menus;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using Xunit;

namespace WebTests.Actions.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryIntegrationTests : WebIntegrationTestBase
    {
        public RemoveMenuCategoryIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Removes_A_Category_The_Menu()
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

            var response = await Delete($"/restaurants/{restaurant.Id.Value}/menu/categories/Pizza");

            Assert.Equal(204, (int)response.StatusCode);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            Assert.Empty(menuDto.Categories);
        }
    }
}
