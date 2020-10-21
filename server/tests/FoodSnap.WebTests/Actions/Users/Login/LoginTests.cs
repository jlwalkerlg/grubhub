using System;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Application.Users.Login;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Login
{
    public class LoginTests : WebTestBase
    {
        public LoginTests(WebAppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Logs_The_User_In()
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

            var response = await Post("/auth/login", new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            });

            Assert.Equal(200, (int)response.StatusCode);

            var loggedInResponse = await Get("/auth/user");
            loggedInResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var correctPassword = "password123";
            var wrongPassword = "wrong_password";

            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                correctPassword);

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                user.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var menu = new Menu(restaurant.Id);

            await fixture.InsertDb(user, restaurant, menu);

            var response = await Post("/auth/login", new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = wrongPassword,
            });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.NotNull(await response.GetErrorMessage());

            var authResponse = await Get("/auth/user");
            Assert.Equal(401, (int)authResponse.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var response = await Post("/auth/login", new LoginCommand
            {
                Email = "",
                Password = "",
            });

            Assert.Equal(422, (int)response.StatusCode);

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("email"));
            Assert.True(errors.ContainsKey("password"));
        }
    }
}
