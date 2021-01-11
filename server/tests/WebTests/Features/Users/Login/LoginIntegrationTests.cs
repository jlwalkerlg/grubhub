using System.Threading.Tasks;
using Web.Features.Users.Login;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginIntegrationTests : IntegrationTestBase
    {
        public LoginIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Logs_The_User_In()
        {
            var user = new User()
            {
                Email = "walker.jlg@gmail.com",
                Password = fixture.Hash("password123"),
            };

            fixture.Insert(user);

            var client = fixture.GetClient();

            var request = new LoginCommand()
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var response = await client.Post(
                "/auth/login",
                request);

            response.StatusCode.ShouldBe(200);

            (await client.Get("/auth/user")).EnsureSuccessStatusCode();
        }
    }
}
