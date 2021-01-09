using System;
using Shouldly;
using Web.Domain;
using Web.Domain.Users;
using Web.Services.Authentication;
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

            authenticator.IsAuthenticated.ShouldBe(true);
            authenticator.UserId.ShouldBe(id);
        }

        [Fact]
        public void It_Returns_Null_When_Not_Authenticated()
        {
            authenticator.IsAuthenticated.ShouldBe(false);
            authenticator.UserId.ShouldBeNull();
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

            var decodedToken = tokenizer.Decode(token).Value;

            authenticator.IsAuthenticated.ShouldBe(true);
            decodedToken.ShouldBe(user.Id.Value.ToString());
            cookieOptions.HttpOnly.ShouldBe(true);
        }

        [Fact]
        public void It_Signs_The_User_Out()
        {
            cookieBagSpy.Add("auth_token", "token");

            authenticator.SignOut();

            var cookieData = cookieBagSpy.Deleted["auth_token"];

            var cookieOptions = cookieData.Options;

            authenticator.IsAuthenticated.ShouldBe(false);
            cookieOptions.HttpOnly.ShouldBe(true);
        }
    }
}
