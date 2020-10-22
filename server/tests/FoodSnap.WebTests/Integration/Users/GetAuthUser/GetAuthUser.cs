using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Users;
using Xunit;

namespace FoodSnap.WebTests.Integration.Users.GetAuthUser
{
    public class GetAuthUserTests : WebTestBase
    {
        public GetAuthUserTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Gets_The_Logged_In_User()
        {
            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                user.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var menu = new Menu(restaurant.Id);

            await fixture.InsertDb(user, restaurant, menu);
            await Login(user);

            var userDto = await Get<UserDto>("/auth/user");
            Assert.Equal(user.Id.Value, userDto.Id);
            Assert.Equal("Jordan Walker", userDto.Name);
            Assert.Equal("walker.jlg@gmail.com", userDto.Email);
            Assert.Equal(restaurant.Id.Value, userDto.RestaurantId);
            Assert.Equal("Chow Main", userDto.RestaurantName);
        }
    }
}
