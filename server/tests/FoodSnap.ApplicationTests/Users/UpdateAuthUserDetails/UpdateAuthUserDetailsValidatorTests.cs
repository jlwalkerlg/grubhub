using System.Threading.Tasks;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using FoodSnap.ApplicationTests.Doubles;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.ApplicationTests.Users.UpdateAuthUserDetails
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
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = user;
            await unitOfWorkSpy.UserRepositorySpy.Add(user);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = name,
                Email = "walker.jlg@gmail.com"
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
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
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = user;
            await unitOfWorkSpy.UserRepositorySpy.Add(user);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = "Jordan Walker",
                Email = email
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Email)));
        }

        [Fact]
        public async Task Disallows_Changing_Email_If_Already_Taken()
        {
            User authUser = new RestaurantManager(
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            User existingUser = new RestaurantManager(
                "Walker Jordan",
                new Email("taken@email.com"),
                "password123");

            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);
            await unitOfWorkSpy.UserRepositorySpy.Add(existingUser);

            authenticatorSpy.User = authUser;

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = "Jordan Walker",
                Email = existingUser.Email.Address
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Email)));
        }
    }
}