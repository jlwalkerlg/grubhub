using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Billing;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Billing.GetOnboardingLink
{
    public class GetOnboardingLinkIntegrationTests : IntegrationTestBase
    {
        public GetOnboardingLinkIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Onboarding_Link()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            fixture.Insert(restaurant);

            var onboardingLink = Guid.NewGuid().ToString();

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
                .Get($"/restaurants/{restaurant.Id}/billing/onboarding/link");

            response.StatusCode.ShouldBe(200);

            var link = await response.GetData<string>();

            link.ShouldBe(onboardingLink);
        }
    }
}
