using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.ChangePassword;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Users.ChangePassword
{
    public class ChangePasswordHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork = new();
        private readonly AuthenticatorSpy authenticator = new();
        private readonly HasherFake hasher = new();
        private readonly ChangePasswordHandler handler;

        public ChangePasswordHandlerTests()
        {
            handler = new ChangePasswordHandler(unitOfWork, authenticator, hasher);
        }

        [Fact]
        public async Task It_Fails_If_The_Current_Password_Is_Incorrect()
        {
            var user = new Customer(
                new UserId(Guid.NewGuid()),
                "Jordan",
                "Walker",
                new Email("email@test.com"),
                hasher.Hash("current-password"));

            await unitOfWork.Users.Add(user);

            await authenticator.SignIn(user);

            var command = new ChangePasswordCommand()
            {
                CurrentPassword = "wrong-password",
                NewPassword = "new-password",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.ValidationError);
            result.Errors.ShouldContainKey(nameof(command.CurrentPassword));
        }
    }
}
