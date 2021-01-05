using System.Threading.Tasks;
using Web.Features.Restaurants.Login;
using Web.Features.Users.Login;
using Xunit;

namespace ApplicationTests.Users.Login
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator validator;

        public LoginValidatorTests()
        {
            validator = new LoginValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("not_an_email")]
        public async Task Disallows_Invalid_Emails(string email)
        {
            var command = new LoginCommand
            {
                Email = email,
                Password = "password123",
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Email)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Passwords(string password)
        {
            var command = new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = password,
            };

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Password)));
        }
    }
}
