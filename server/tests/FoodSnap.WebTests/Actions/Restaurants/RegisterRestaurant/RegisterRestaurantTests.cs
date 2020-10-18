using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Menus;
using FoodSnap.Web.Actions.Restaurants;
using FoodSnap.Web.Actions.Users;
using FoodSnap.WebTests.Doubles;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantTests : WebTestBase
    {
        public RegisterRestaurantTests(TestWebApplicationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Registers_A_Restaurant_And_A_Manager()
        {
            var response = await fixture.Post("/restaurants/register", new RegisterRestaurantCommand
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK"
            });

            Assert.Equal(201, (int)response.StatusCode);

            await fixture.ExecuteDb(async db =>
            {
                var user = await db.Users.SingleAsync();
                fixture.Login(user);
            });

            var user = await fixture.Get<UserDto>("/auth/user");
            Assert.Equal("Jordan Walker", user.Name);
            Assert.Equal("walker.jlg@gmail.com", user.Email);
            Assert.NotEqual("password123", user.Password);
            Assert.Equal(UserRole.RestaurantManager.ToString(), user.Role);

            var restaurant = await fixture.Get<RestaurantDto>($"/restaurants/{user.RestaurantId}");
            Assert.Equal(user.Id, restaurant.ManagerId);
            Assert.Equal("Chow Main", restaurant.Name);
            Assert.Equal("01234567890", restaurant.PhoneNumber);
            Assert.Equal(RestaurantStatus.PendingApproval.ToString(), restaurant.Status);
            Assert.Equal(GeocoderStub.Address, restaurant.Address);
            Assert.Equal(GeocoderStub.Latitude, restaurant.Latitude);
            Assert.Equal(GeocoderStub.Longitude, restaurant.Longitude);

            var menu = await fixture.Get<MenuDto>($"/restaurants/{restaurant.Id}/menu");
            Assert.Empty(menu.Categories);
        }
    }
}
