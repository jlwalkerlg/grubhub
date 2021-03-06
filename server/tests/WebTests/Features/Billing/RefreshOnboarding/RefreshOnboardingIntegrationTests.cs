using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Billing;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingIntegrationTests : IntegrationTestBase
    {
        public RefreshOnboardingIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Redirects_To_The_Onboarding_Link()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            var onboardingLink = "http://localhost:5000";

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IBillingService>(
                    new BillingServiceSpy()
                    {
                        AccountId = billingAccount.Id,
                        OnboardingLink = onboardingLink,
                    });
                });
            });

            Insert(restaurant);

            var response = await factory
                .GetAuthenticatedClient(restaurant.ManagerId)
                .Get($"/stripe/onboarding/refresh?restaurant_id={restaurant.Id}");

            response.StatusCode.ShouldBe(302);
            response.Headers.Location.OriginalString.ShouldBe(onboardingLink);
        }
    }
}
