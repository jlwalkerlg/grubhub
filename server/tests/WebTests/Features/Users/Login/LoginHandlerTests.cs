using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.Login;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginHandlerTests
    {
        private readonly HasherFake hasherFake;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly LoginHandler handler;

        public LoginHandlerTests()
        {
            hasherFake = new HasherFake();
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new LoginHandler(
                unitOfWorkSpy,
                authenticatorSpy,
                hasherFake);
        }

        [Fact]
        public async Task It_Signs_The_User_In()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                hasherFake.Hash("password123"));

            unitOfWorkSpy.UserRepositorySpy.Users.Add(user);

            var command = new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };
            var result = await handler.Handle(command, default);

            Assert.Same(user.Id, authenticatorSpy.UserId);
            Assert.True(result);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_User_Not_Found()
        {
            var command = new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };
            var result = await handler.Handle(command, default);

            Assert.False(result);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_Password_Doesnt_Match()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                hasherFake.Hash("password123"));

            unitOfWorkSpy.UserRepositorySpy.Users.Add(user);

            var command = new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "wrong_password",
            };
            var result = await handler.Handle(command, default);

            Assert.False(result);
        }
    }
}
