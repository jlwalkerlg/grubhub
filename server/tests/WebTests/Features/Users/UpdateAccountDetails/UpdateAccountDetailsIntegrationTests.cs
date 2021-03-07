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
        public async Task It_Updates_The_Customers_Account_Details()
        {
            var customer = new User()
            {
                Role = UserRole.Customer,
            };

            Insert(customer);

            var command = new UpdateAccountDetailsCommand()
            {
                Name = Guid.NewGuid().ToString(),
                MobileNumber = "07098765432",
            };

            var response = await factory
                .GetAuthenticatedClient(customer)
                .Put("/account/details", command);

            response.StatusCode.ShouldBe(200);

            Reload(customer);

            customer.Name.ShouldBe(command.Name);
            customer.MobileNumber.ShouldBe(command.MobileNumber);
        }
    }
}
