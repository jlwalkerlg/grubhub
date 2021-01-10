using Shouldly;
using System.Threading.Tasks;
using Web.Features.Restaurants.Login;
using Web.Features.Users.Login;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("not_an_email")]
        public async Task Disallows_Invalid_Emails(string email)
        {
            var command = new LoginCommand()
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
        public async Task Disallows_Invalid_Passwords(string password)
        {
            var command = new LoginCommand()
            {
                Password = password,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Password));
        }
    }
}
