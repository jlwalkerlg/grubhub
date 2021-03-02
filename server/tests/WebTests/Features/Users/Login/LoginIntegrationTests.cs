using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Web.Features.Users.Login;
using Web.Services.Hashing;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.Login
{
    public class LoginIntegrationTests : IntegrationTestBase
    {
        private readonly IHasher hasher;

        public LoginIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
            hasher = factory.Services.GetRequiredService<IHasher>();
        }

        [Fact]
        public async Task It_Logs_The_User_In()
        {
            var user = new User()
            {
                Email = "walker.jlg@gmail.com",
                Password = hasher.Hash("password123"),
            };

            Insert(user);

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                });
            });

            var client = factory.GetClient();

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
