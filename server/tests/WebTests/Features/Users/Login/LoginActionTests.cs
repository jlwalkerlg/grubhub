using System.Threading.Tasks;
using Shouldly;
using Web.Features.Users.Login;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginActionTests : HttpTestBase
    {
        public LoginActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new LoginCommand()
            {
                Email = "",
                Password = "",
            };

            var response = await fixture.GetClient().Post(
                "/auth/login",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = await response.GetErrors();

            errors.ShouldContainKey("email");
            errors.ShouldContainKey("password");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new LoginCommand()
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var response = await fixture.GetClient().Post(
                "/auth/login",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().Result.ShouldBe(fixture.HandlerErrorMessage);

            (await fixture.GetClient().Get("/auth/user")).StatusCode.ShouldBe(401);
        }
    }
}
