using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Users.ChangePassword;
using Web.Services.Hashing;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.ChangePassword
{
    public class ChangePasswordIntegrationTests : IntegrationTestBase
    {
        private readonly IHasher hasher;

        public ChangePasswordIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
            hasher = factory.Services.GetRequiredService<IHasher>();
        }

        [Fact]
        public async Task It_Changes_The_Users_Password()
        {
            var user = new User();

            Insert(user);

            var command = new ChangePasswordCommand()
            {
                Password = Guid.NewGuid().ToString(),
            };

            var response = await factory.GetAuthenticatedClient(user).Put(
                "/account/password",
                command);

            response.StatusCode.ShouldBe(200);

            Reload(user);

            hasher.CheckMatch(command.Password, user.Password).ShouldBeTrue();
        }
    }
}
