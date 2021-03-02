using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Web.Features.Users.Login;
using Web.Services.Hashing;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.Logout
{
    public class LogoutIntegrationTests : IntegrationTestBase
    {
        private readonly IHasher hasher;

        public LogoutIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
            hasher = factory.Services.GetRequiredService<IHasher>();
        }

        [Fact]
        public async Task It_Logs_The_User_Out()
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
