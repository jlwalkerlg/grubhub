using System;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.WebTests.Integration.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsTests : WebIntegrationTestBase
    {
        public UpdateAuthUserDetailsTests(WebAppIntegrationTestFixture fixture) : base(fixture)
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
