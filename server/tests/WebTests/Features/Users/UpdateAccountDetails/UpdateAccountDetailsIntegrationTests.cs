using System;
using System.Threading.Tasks;
using Web.Features.Users.UpdateAccountDetails;
using Shouldly;
using Web.Domain.Users;
using Xunit;
using User = WebTests.TestData.User;

namespace WebTests.Features.Users.UpdateAccountDetails
{
    public class UpdateAccountDetailsIntegrationTests : IntegrationTestBase
    {
        public UpdateAccountDetailsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Users_Account_Details()
        {
            var user = new User()
            {
                Role = UserRole.Customer,
            };

            Insert(user);

            var command = new UpdateAccountDetailsCommand()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MobileNumber = "07098765432",
            };

            var response = await factory
                .GetAuthenticatedClient(user)
                .Put("/account/details", command);

            if (!response.IsSuccessStatusCode) throw new Exception(response.Content.ReadAsStringAsync().Result);

            response.StatusCode.ShouldBe(200);

            Reload(user);

            user.FirstName.ShouldBe(command.FirstName);
            user.LastName.ShouldBe(command.LastName);
            user.MobileNumber.ShouldBe(command.MobileNumber);
        }
    }
}
