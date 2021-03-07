using System.Threading.Tasks;
using Shouldly;
using Web.Features.Users.RegisterCustomer;
using Xunit;

namespace WebTests.Features.Users.Register
{
    public class RegisterValidatorTests
    {
        private readonly RegisterCustomerValidator validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Names(string name)
        {
            var command = new RegisterCustomerCommand()
            {
                FirstName = name,
                LastName = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.FirstName));
            result.Errors.ShouldContainKey(nameof(command.LastName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public async Task Disallows_Invalid_Emails(string email)
        {
            var command = new RegisterCustomerCommand()
            {
                Email = email,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Email));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1234567")]
        public async Task Disallows_Invalid_Passwords(string password)
        {
            var command = new RegisterCustomerCommand()
            {
                Password = password,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Password));
        }
    }
}
