using System.Threading.Tasks;
using Shouldly;
using Web.Features.Users.UpdateAuthUserDetails;
using Xunit;

namespace WebTests.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsValidatorTests
    {
        private readonly UpdateAuthUserDetailsValidator validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Manager_Names(string name)
        {
            var command = new UpdateAuthUserDetailsCommand()
            {
                Name = name,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Name));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public async Task Disallows_Invalid_Manager_Emails(string email)
        {
            var command = new UpdateAuthUserDetailsCommand()
            {
                Email = email
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Email));
        }
    }
}
