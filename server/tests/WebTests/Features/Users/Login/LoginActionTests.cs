using Shouldly;
using System.Threading.Tasks;
using Web.Features.Users.Login;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginActionTests : ActionTestBase
    {
        public LoginActionTests(ActionTestWebApplicationFactory factory) : base(factory)
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

            var response = await GetClient().Post(
                "/auth/login",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

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

            var response = await GetClient().Post(
                "/auth/login",
                request);

            response.StatusCode.ShouldBe(400);

            (await GetClient().Get("/auth/user")).StatusCode.ShouldBe(401);
        }
    }
}
