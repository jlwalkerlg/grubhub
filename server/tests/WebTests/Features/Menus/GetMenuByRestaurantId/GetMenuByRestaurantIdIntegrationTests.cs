using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus;
using Xunit;

namespace WebTests.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdIntegrationTests : WebIntegrationTestBase
    {
        public GetMenuByRestaurantIdIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Gets_The_Menu()
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
            menu.GetCategory("Pizza").AddItem("Margherita", "Cheese & tomato", new Money(10));

            await fixture.InsertDb(manager, restaurant, menu);

            var menuDto = await Get<MenuDto>($"/restaurants/{restaurant.Id.Value}/menu");
            Assert.Equal(menu.RestaurantId.Value, menuDto.RestaurantId);
            Assert.Single(menu.Categories);

            var categoryDto = menuDto.Categories.Single();
            Assert.Equal("Pizza", categoryDto.Name);
            Assert.Single(categoryDto.Items);

            var itemDto = categoryDto.Items.Single();
            Assert.Equal("Margherita", itemDto.Name);
            Assert.Equal("Cheese & tomato", itemDto.Description);
            Assert.Equal(10m, itemDto.Price);
        }
    }
}
