using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Hashing;
using FoodSnap.Web.Actions.Users.Login;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Auth;
using FoodSnap.Web.Queries.Restaurants;
using FoodSnap.Web.Queries.Users;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Login
{
    public class LoginActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly Hasher hasher;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly LoginAction action;

        public LoginActionTests()
        {
            mediatorSpy = new MediatorSpy();
            hasher = new Hasher();
            authenticatorSpy = new AuthenticatorSpy();

            action = new LoginAction(
                mediatorSpy,
                hasher,
                authenticatorSpy);
        }

        [Fact]
        public async Task It_Returns_400_If_User_Not_Found()
        {
            mediatorSpy.Result = Result<AuthDataDto>.Ok(null);

            var request = new LoginRequest
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123"
            };

            var result = await action.Login(request) as BadRequestObjectResult;

            Assert.IsType<ErrorEnvelope>(result.Value);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task It_Returns_400_If_Password_Is_Incorrect()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = hasher.Hash("password123"),
                Role = "Admin"
            };
            var restaurant = new RestaurantDto
            {
                Id = Guid.NewGuid(),
                ManagerId = user.Id,
                Name = "Chow Main",
                PhoneNumber = "01234567890",
                AddressLine1 = "12 China Road",
                AddressLine2 = "",
                Town = "China Town",
                Postcode = "CH111NA",
                Latitude = 1,
                Longitude = 1,
                Status = "Pending",
            };
            mediatorSpy.Result = Result.Ok(new AuthDataDto
            {
                User = user,
                Restaurant = restaurant,
            });

            var request = new LoginRequest
            {
                Email = "walker.jlg@gmail.com",
                Password = "incorrectpassword"
            };

            var result = await action.Login(request) as BadRequestObjectResult;

            Assert.IsType<ErrorEnvelope>(result.Value);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task It_Returns_The_User_And_Restaurant_On_Success()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = hasher.Hash("password123"),
                Role = "Admin"
            };
            var restaurant = new RestaurantDto
            {
                Id = Guid.NewGuid(),
                ManagerId = user.Id,
                Name = "Chow Main",
                PhoneNumber = "01234567890",
                AddressLine1 = "12 China Road",
                AddressLine2 = "",
                Town = "China Town",
                Postcode = "CH111NA",
                Latitude = 1,
                Longitude = 1,
                Status = "Pending",
            };
            var response = new AuthDataDto
            {
                User = user,
                Restaurant = restaurant,
            };
            mediatorSpy.Result = Result.Ok(response);

            var request = new LoginRequest
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123"
            };

            var result = await action.Login(request) as ObjectResult;
            Assert.Equal(200, result.StatusCode);

            var envelope = result.Value as DataEnvelope;
            Assert.Same(response, envelope.Data);
        }

        [Fact]
        public async Task It_Signs_The_The_User_In_On_Success()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = hasher.Hash("password123"),
                Role = "Admin"
            };
            var restaurant = new RestaurantDto
            {
                Id = Guid.NewGuid(),
                ManagerId = user.Id,
                Name = "Chow Main",
                PhoneNumber = "01234567890",
                AddressLine1 = "12 China Road",
                AddressLine2 = "",
                Town = "China Town",
                Postcode = "CH111NA",
                Latitude = 1,
                Longitude = 1,
                Status = "Pending",
            };
            mediatorSpy.Result = Result.Ok(new AuthDataDto
            {
                User = user,
                Restaurant = restaurant,
            });

            var request = new LoginRequest
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123"
            };

            await action.Login(request);

            Assert.Same(user, authenticatorSpy.User);
        }
    }
}
