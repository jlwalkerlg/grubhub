using System.Threading.Tasks;
using Web.Features.Users.Login;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.Logout
{
    public class LogoutIntegrationTests : IntegrationTestBase
    {
        public LogoutIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Logs_The_User_Out()
        {
            var user = new User()
            {
                Email = "walker.jlg@gmail.com",
                Password = fixture.Hash("password123"),
            };

            fixture.Insert(user);

            var client = fixture.CreateClient();

            var loginRequest = new LoginCommand()
            {
                Email = user.Email,
                Password = "password123"
            };

            (await client.Post("/auth/login", loginRequest)).EnsureSuccessStatusCode();

            (await client.Get("/auth/user")).EnsureSuccessStatusCode();

            (await client.Post("/auth/logout")).EnsureSuccessStatusCode();

            (await client.Get("/auth/user")).StatusCode.ShouldBe(401);
        }
    }
}
