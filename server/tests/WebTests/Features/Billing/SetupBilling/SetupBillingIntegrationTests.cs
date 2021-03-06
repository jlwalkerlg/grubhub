using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Billing;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Billing.SetupBilling
{
    public class SetupBillingIntegrationTests : IntegrationTestBase
    {
        public SetupBillingIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Onboarding_Link()
        {
            var restaurant = new Restaurant();
            var billingAccount = restaurant.BillingAccount;

            var onboardingLink = Guid.NewGuid().ToString();

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
                .Post($"/restaurants/{restaurant.Id}/billing/setup");

            response.StatusCode.ShouldBe(200);

            var link = await response.GetData<string>();

            link.ShouldBe(onboardingLink);
        }
    }
}
