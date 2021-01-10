using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.UpdateAuthUserDetails;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

namespace WebTests.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly UpdateAuthUserDetailsHandler handler;

        public UpdateAuthUserDetailsHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new UpdateAuthUserDetailsHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Returns_A_Validation_Error_If_The_Email_Is_Already_Taken()
        {
            var authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var existingUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Bruno",
                new Email("taken@gmail.com"),
                "password123");

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.UserRepositorySpy.Add(existingUser);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateAuthUserDetailsCommand()
            {
                Name = "Jordan Walker",
                Email = "taken@gmail.com",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.ValidationError);
            result.Errors.ShouldContainKey(nameof(command.Email));
        }
    }
}
