using System;
using System.Threading.Tasks;
using FoodSnap.Shared;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Users;
using FoodSnap.Web.Actions.Users.GetAuthUser;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.GetAuthData
{
    public class GetAuthDataActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly GetAuthUserAction action;

        public GetAuthDataActionTests()
        {
            mediatorSpy = new MediatorSpy();

            authenticatorSpy = new AuthenticatorSpy();

            action = new GetAuthUserAction(mediatorSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Returns_401_If_Unauthorised()
        {
            var result = await action.Execute() as StatusCodeResult;

            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task It_Returns_The_User_With_Auth_Data()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = user;

            var userDto = new UserDto
            {
                Id = user.Id.Value,
                Name = user.Name,
                Email = user.Email.Address,
                Password = user.Password,
                Role = user.Role.ToString(),
                RestaurantId = Guid.NewGuid(),
                RestaurantName = "Chow Main",
            };
            mediatorSpy.Result = Result.Ok(userDto);

            var result = await action.Execute() as ObjectResult;
            var envelope = result.Value as DataEnvelope;

            Assert.Equal(200, result.StatusCode);
            Assert.Same(userDto, envelope.Data);
        }
    }
}
