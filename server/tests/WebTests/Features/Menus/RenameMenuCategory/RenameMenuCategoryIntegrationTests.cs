using System.Linq;
using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Menus.RenameMenuCategory;
using Xunit;
using Web.Features.Menus;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryIntegrationTests : WebIntegrationTestBase
    {
        public RenameMenuCategoryIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
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
