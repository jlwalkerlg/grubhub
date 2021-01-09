using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdIntegrationTests : IntegrationTestBase
    {
        public GetMenuByRestaurantIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Gets_The_Menu()
        {
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var item = new MenuItem()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            };

            var category = new MenuCategory()
            {
                Name = "Pizza",
                Items = new() { item },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = new() { category },
            };

            fixture.Insert(manager, restaurant, menu);

            var response = await fixture.GetClient().Get($"/restaurants/{restaurant.Id}/menu");

            response.StatusCode.ShouldBe(200);

            var menuDto = await response.GetData<MenuDto>();

            menuDto.RestaurantId.ShouldBe(restaurant.Id);
            menuDto.Categories.ShouldHaveSingleItem();

            var categoryDto = menuDto.Categories.First();
            categoryDto.Name.ShouldBe(category.Name);
            categoryDto.Items.ShouldHaveSingleItem();

            var itemDto = categoryDto.Items.First();
            itemDto.Name.ShouldBe(item.Name);
            itemDto.Description.ShouldBe(item.Description);
            itemDto.Price.ShouldBe(item.Price);
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var response = await fixture.GetClient().Get($"/restaurants/{Guid.NewGuid()}/menu");

            response.StatusCode.ShouldBe(404);
        }
    }
}
