using System;
using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Features.Users.UpdateDeliveryAddress;
using Shouldly;
using Xunit;
using User = WebTests.TestData.User;

namespace WebTests.Features.Users.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressIntegrationTests : IntegrationTestBase
    {
        public UpdateDeliveryAddressIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Customers_Delivery_Address()
        {
            var user = new User()
            {
                Role = UserRole.Customer,
            };

            Insert(user);

            var command = new UpdateDeliveryAddressCommand()
            {
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                Postcode = "WP12 1MR",
            };

            var response = await factory.GetAuthenticatedClient(user).Put("/account/delivery-address", command);

            response.StatusCode.ShouldBe(200);

            Reload(user);

            user.AddressLine1.ShouldBe(command.AddressLine1);
            user.AddressLine2.ShouldBe(command.AddressLine2);
            user.City.ShouldBe(command.City);
            user.Postcode.ShouldBe(command.Postcode);
        }
    }
}
