using System.Threading.Tasks;
using Application.Users.Login;
using Xunit;

namespace WebTests.Actions.Users.Login
{
    public class LoginTests : WebActionTestBase
    {
        public LoginTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Post("/auth/login", new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.NotNull(await response.GetErrorMessage());

            var authResponse = await Get("/auth/user");
            Assert.Equal(401, (int)authResponse.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var response = await Post("/auth/login", new LoginCommand
            {
                Email = "",
                Password = "",
            });

            Assert.Equal(422, (int)response.StatusCode);

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("email"));
            Assert.True(errors.ContainsKey("password"));
        }
    }
}
