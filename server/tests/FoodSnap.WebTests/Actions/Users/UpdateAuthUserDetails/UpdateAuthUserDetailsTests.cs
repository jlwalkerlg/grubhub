using System;
using System.Threading.Tasks;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsTests : WebActionTestBase
    {
        public UpdateAuthUserDetailsTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Put("/auth/user", new UpdateAuthUserDetailsCommand
            {
                Name = "Bruno",
                Email = "bruno@gmail.com",
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            await Login(user);

            var response = await Put("/auth/user", new UpdateAuthUserDetailsCommand
            {
                Name = "",
                Email = "",
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("name"));
            Assert.True(errors.ContainsKey("email"));
        }
    }
}
