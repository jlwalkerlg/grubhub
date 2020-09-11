using System;
using FoodSnap.Web.Queries.Users;
using FoodSnap.Web.Services.Authentication;
using FoodSnap.WebTests.Doubles;
using Xunit;

namespace FoodSnap.WebTests.Services.Authentication
{
    public class AuthenticatorTests
    {
        private readonly IAuthenticator authenticator;
        private readonly TokenizerFake tokenizer;
        private readonly CookieBagSpy cookieBagSpy;

        public AuthenticatorTests()
        {
            tokenizer = new TokenizerFake();
            cookieBagSpy = new CookieBagSpy();

            authenticator = new Authenticator(
                tokenizer,
                cookieBagSpy);
        }

        [Fact]
        public void It_Gets_The_User_Id_When_Authenticated()
        {
            var id = Guid.NewGuid();

            var token = tokenizer.Encode(id.ToString());
            cookieBagSpy.Add("auth_token", token);

            Assert.True(authenticator.IsAuthenticated);
            Assert.Equal(id, authenticator.UserId);
        }

        [Fact]
        public void It_Returns_Empty_User_Id_If_Not_Authenticated()
        {
            Assert.False(authenticator.IsAuthenticated);
            Assert.Equal(Guid.Empty, authenticator.UserId);
        }

        [Fact]
        public void It_Signs_The_User_In()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
                Role = "RestaurantManager",
            };

            authenticator.SignIn(user);

            var cookieData = cookieBagSpy.Cookies["auth_token"];
            var token = cookieData.Value;
            var cookieOptions = cookieData.Options;

            Assert.True(authenticator.IsAuthenticated);
            Assert.Equal(user.Id.ToString(), tokenizer.Decode(token).Value);
            Assert.True(cookieOptions.HttpOnly);
        }

        [Fact]
        public void It_Signs_The_User_Out()
        {
            cookieBagSpy.Add("auth_token", "token");

            authenticator.SignOut();

            var cookieData = cookieBagSpy.Deleted["auth_token"];
            var cookieOptions = cookieData.Options;

            Assert.False(authenticator.IsAuthenticated);
            Assert.True(cookieOptions.HttpOnly);
        }
    }
}