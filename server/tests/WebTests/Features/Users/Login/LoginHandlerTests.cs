using Shouldly;
using System;
using System.Threading.Tasks;
using Web;
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
        public async Task It_Fails_If_The_User_Is_Not_Found()
        {
            var command = new LoginCommand()
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Passwords_Dont_Match()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan",
                "Walker",
                new Email("walker.jlg@gmail.com"),
                hasherFake.Hash("password123"));

            await unitOfWorkSpy.UserRepositorySpy.Add(user);

            var command = new LoginCommand()
            {
                Email = user.Email.Address,
                Password = "wrong_password",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
