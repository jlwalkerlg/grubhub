using System;
using System.Threading.Tasks;
using Web.Features.Users;
using Web.Features.Users.UpdateAuthUserDetails;
using Web.Domain;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Actions.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsIntegrationTests : WebIntegrationTestBase
    {
        public UpdateAuthUserDetailsIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Auth_Users_Details()
        {
            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            await fixture.InsertDb(user);
            await Login(user);

            var response = await Put("/auth/user", new UpdateAuthUserDetailsCommand
            {
                Name = "Bruno",
                Email = "bruno@gmail.com",
            });

            var userDto = await Get<UserDto>("/auth/user");
            Assert.Equal("Bruno", userDto.Name);
            Assert.Equal("bruno@gmail.com", userDto.Email);
        }
    }
}
