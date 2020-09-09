using System;
using FoodSnap.Web.Queries.Users;
using FoodSnap.Web.Services.Authentication;
using FoodSnap.WebTests.Doubles;
using Xunit;

namespace FoodSnap.WebTests.Services.Authentication
{
    public class AuthenticatorTests
    {
        private readonly Authenticator authenticator;
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
        public void It_Gets_The_User_Id()
        {
            var id = Guid.NewGuid();
            var token = tokenizer.Encode(id.ToString());

            cookieBagSpy.Add("auth_token", token);

            Assert.Equal(id, authenticator.GetUserId());
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

            var token = cookieBagSpy.Get("auth_token");
            Assert.Equal(user.Id.ToString(), tokenizer.Decode(token).Value);
        }
    }
}
