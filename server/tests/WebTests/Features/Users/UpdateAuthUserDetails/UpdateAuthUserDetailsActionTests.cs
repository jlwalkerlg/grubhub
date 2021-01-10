using Shouldly;
using System.Threading.Tasks;
using Web.Features.Users.UpdateAuthUserDetails;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsActionTests : HttpTestBase
    {
        public UpdateAuthUserDetailsActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateAuthUserDetailsCommand()
            {
                Name = "Bruno",
                Email = "bruno@gmail.com",
            };

            var response = await fixture.GetClient().Put(
                "/auth/user",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var user = new User();

            var request = new UpdateAuthUserDetailsCommand()
            {
                Name = "",
                Email = "",
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Put(
                "/auth/user",
                request);

            var errors = response.GetErrors();

            errors.ShouldContainKey("name");
            errors.ShouldContainKey("email");
        }
    }
}
