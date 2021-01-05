using System.Linq;
using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.AddMenuCategory;
using Xunit;
using Web.Features.Menus;

namespace WebTests.Actions.Menus.AddMenuCategory
{
    public class AddMenuCategoryIntegrationTests : WebIntegrationTestBase
    {
        public AddMenuCategoryIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
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
    }
}
