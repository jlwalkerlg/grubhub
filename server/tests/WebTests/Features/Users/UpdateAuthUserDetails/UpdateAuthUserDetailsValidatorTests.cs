using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.UpdateAuthUserDetails;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsValidatorTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateAuthUserDetailsValidator validator;

        public UpdateAuthUserDetailsValidatorTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            validator = new UpdateAuthUserDetailsValidator(authenticatorSpy, unitOfWorkSpy);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Manager_Names(string name)
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(user);
            await unitOfWorkSpy.UserRepositorySpy.Add(user);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = name,
                Email = "walker.jlg@gmail.com"
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Name)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public async Task Disallows_Invalid_Manager_Emails(string email)
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(user);
            await unitOfWorkSpy.UserRepositorySpy.Add(user);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = "Jordan Walker",
                Email = email
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Email)));
        }

        [Fact]
        public async Task Disallows_Changing_Email_If_Already_Taken()
        {
            User authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            User existingUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Walker Jordan",
                new Email("taken@email.com"),
                "password123");

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.UserRepositorySpy.Add(existingUser);

            authenticatorSpy.SignIn(authUser);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = "Jordan Walker",
                Email = existingUser.Email.Address
            };

            var result = await validator.Validate(command);

            Assert.False(result);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Email)));
        }
    }
}
