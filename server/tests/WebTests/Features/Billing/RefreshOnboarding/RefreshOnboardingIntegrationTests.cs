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

            fixture.Insert(restaurant);

            var onboardingLink = "http://localhost:5000";

            using var factory = fixture.CreateFactory(services =>
            {
                services.AddSingleton<IBillingService>(
                    new BillingServiceSpy()
                    {
                        AccountId = billingAccount.Id,
                        OnboardingLink = onboardingLink,
                    });
            });

            var client = new HttpTestClient(factory);

            var response = await client
                .Authenticate(restaurant.ManagerId)
                .Get($"/stripe/onboarding/refresh?restaurant_id={restaurant.Id}");

            response.StatusCode.ShouldBe(302);
            response.Headers.Location.OriginalString.ShouldBe(onboardingLink);
        }
    }
}
