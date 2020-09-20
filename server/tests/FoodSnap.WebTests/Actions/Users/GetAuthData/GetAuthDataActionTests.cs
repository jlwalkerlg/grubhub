using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Users.GetAuthData;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Auth;
using FoodSnap.Web.Queries.Users;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.GetAuthData
{
    public class GetAuthDataActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly GetAuthDataAction action;

        public GetAuthDataActionTests()
        {
            mediatorSpy = new MediatorSpy();

            authenticatorSpy = new AuthenticatorSpy();

            action = new GetAuthDataAction(mediatorSpy, authenticatorSpy);
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
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = user;

            var data = new AuthDataDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email.Address,
                    Password = user.Password,
                    Role = user.Role.ToString(),
                }
            };
            mediatorSpy.Result = Result.Ok(data);

            var result = await action.Execute() as ObjectResult;
            var envelope = result.Value as DataEnvelope;

            Assert.Equal(200, result.StatusCode);
            Assert.Same(data, envelope.Data);
        }
    }
}
