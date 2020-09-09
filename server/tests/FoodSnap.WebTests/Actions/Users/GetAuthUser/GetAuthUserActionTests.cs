using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Web.Actions.Users.GetAuthUser;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.GetUserById;
using FoodSnap.Web.Queries.Users;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.GetAuthUser
{
    public class GetAuthUserActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly GetAuthUserAction action;

        public GetAuthUserActionTests()
        {
            mediatorSpy = new MediatorSpy();
            authenticatorSpy = new AuthenticatorSpy();

            action = new GetAuthUserAction(mediatorSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Queries_For_The_Right_User()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
                Role = "RestaurantManager",
            };

            authenticatorSpy.User = user;
            mediatorSpy.Result = Result.Ok(user);

            await action.GetAuthUser();

            var query = mediatorSpy.Request as GetUserByIdQuery;

            Assert.Equal(user.Id, query.Id);
        }

        [Fact]
        public async Task It_Returns_403_If_Authenticator_Returns_Null_Id()
        {
            authenticatorSpy.User = null;

            var response = await action.GetAuthUser() as StatusCodeResult;

            Assert.Equal(403, response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_The_User_If_Successful()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
                Role = "RestaurantManager",
            };

            authenticatorSpy.User = user;
            mediatorSpy.Result = Result.Ok(user);

            var response = await action.GetAuthUser() as ObjectResult;
            var envelope = response.Value as DataEnvelope;

            Assert.Equal(200, response.StatusCode);
            Assert.Same(user, envelope.Data);
        }

        [Fact]
        public async Task It_Returns_403_If_No_Such_User_Exists()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
                Role = "RestaurantManager",
            };

            authenticatorSpy.User = user;
            mediatorSpy.Result = Result.Ok<UserDto>(null);

            var response = await action.GetAuthUser() as StatusCodeResult;

            Assert.Equal(403, response.StatusCode);
        }
    }
}
