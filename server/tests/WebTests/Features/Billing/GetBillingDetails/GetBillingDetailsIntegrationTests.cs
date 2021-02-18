using System.Threading.Tasks;
using Shouldly;
using Web.Features.Billing.GetBillingDetails;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Billing.GetBillingDetails
{
    public class GetBillingDetailsIntegrationTests : IntegrationTestBase
    {
        public GetBillingDetailsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthenticated()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            Insert(restaurant);

            var response = await factory.GetClient().Get(
                $"/restaurants/{restaurant.Id}/billing");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            Insert(restaurant);

            var response = await factory.GetAuthenticatedClient().Get(
                $"/restaurants/{restaurant.Id}/billing");

            response.StatusCode.ShouldBe(403);
        }

        [Fact]
        public async Task It_Gets_The_Billing_Details_For_A_Restaurant()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            Insert(restaurant);

            var response = await factory
                .GetAuthenticatedClient(restaurant.ManagerId)
                .Get($"/restaurants/{restaurant.Id}/billing");

            response.StatusCode.ShouldBe(200);

            var details = await response.GetData<BillingDetails>();

            details.Id.ShouldBe(billingAccount.Id);
            details.RestaurantId.ShouldBe(restaurant.Id);
            details.IsBillingEnabled.ShouldBe(billingAccount.IsBillingEnabled);
        }

        [Fact]
        public async Task It_Returns_Null_If_No_Account_Exists()
        {
            var restaurant = new Restaurant()
            {
                BillingAccount = null,
            };

            Insert(restaurant);

            var response = await factory
                .GetAuthenticatedClient(restaurant.ManagerId)
                .Get($"/restaurants/{restaurant.Id}/billing");

            response.StatusCode.ShouldBe(200);

            var details = await response.GetData<BillingDetails>();

            details.ShouldBe(null);
        }
    }
}
