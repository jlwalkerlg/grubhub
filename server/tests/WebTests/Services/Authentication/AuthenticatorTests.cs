using System;
using Web.Services.Authentication;
using Web.Domain;
using Web.Domain.Users;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Services.Authentication
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
            var id = new UserId(Guid.NewGuid());

            var token = tokenizer.Encode(id.Value.ToString());
            cookieBagSpy.Add("auth_token", token);

            Assert.True(authenticator.IsAuthenticated);
            Assert.Equal(id, authenticator.UserId);
        }

        [Fact]
        public void It_Returns_A_Null_User_Id_If_Not_Authenticated()
        {
            Assert.False(authenticator.IsAuthenticated);
            Assert.Null(authenticator.UserId);
        }

        [Fact]
        public void It_Signs_The_User_In()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            authenticator.SignIn(user);

            var cookieData = cookieBagSpy.Cookies["auth_token"];
            var token = cookieData.Value;
            var cookieOptions = cookieData.Options;

            Assert.True(authenticator.IsAuthenticated);
            Assert.Equal(user.Id.Value.ToString(), tokenizer.Decode(token).Value);
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
