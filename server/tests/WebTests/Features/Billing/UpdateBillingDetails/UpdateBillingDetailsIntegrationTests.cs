using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Features.Billing.UpdateBillingDetails;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Billing.UpdateBillingDetails
{
    public class UpdateBillingDetailsIntegrationTests : IntegrationTestBase
    {
        public UpdateBillingDetailsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_Billing_Account_Is_Not_Found()
        {
            var command = new UpdateBillingDetailsCommand()
            {
                BillingAccountId = Guid.NewGuid().ToString(),
                IsBillingEnabled = true,
            };

            var result = await factory.Send(command);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Updates_Billing()
        {
            var billingAccount = new BillingAccount()
            {
                IsBillingEnabled = false,
            };

            var restaurant = new Restaurant()
            {
                BillingAccount = billingAccount,
            };

            Insert(restaurant);

            var command = new UpdateBillingDetailsCommand()
            {
                BillingAccountId = billingAccount.Id,
                IsBillingEnabled = true,
            };

            var result = await factory.Send(command);

            result.ShouldBeSuccessful();

            var found = UseTestDbContext(db => db.BillingAccounts.Single());

            found.IsBillingEnabled.ShouldBe(true);
        }
    }
}
