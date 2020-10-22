using System;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Application.Users.Login;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.WebTests.Integration.Users.Logout
{
    public class LogoutTests : WebTestBase
    {
        public LogoutTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Logs_The_User_Out()
        {
            string password = null;
            await fixture.ExecuteService<IHasher>(hasher =>
            {
                password = hasher.Hash("password123");
                return Task.CompletedTask;
            });

            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                password);

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                user.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var menu = new Menu(restaurant.Id);

            await fixture.InsertDb(user, restaurant, menu);

            var loginResponse = await Post("/auth/login", new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            });
            loginResponse.EnsureSuccessStatusCode();

            var loggedInResponse = await Get("/auth/user");
            loggedInResponse.EnsureSuccessStatusCode();

            var logoutResponse = await Post("/auth/logout");
            logoutResponse.EnsureSuccessStatusCode();

            var loggedOutResponse = await Get("/auth/user");
            Assert.Equal(401, (int)loggedOutResponse.StatusCode);
        }
    }
}
