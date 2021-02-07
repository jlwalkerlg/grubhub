using System.Threading.Tasks;
using Stripe;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Features.Billing;

namespace Web.Services.Billing
{
    public class StripeBillingService : IBillingService
    {
        private readonly Config config;

        public StripeBillingService(Config config)
        {
            this.config = config;
        }

        public async Task<string> CreateAccount(Restaurant restaurant)
        {
            var service = new AccountService();

            var account = await service.CreateAsync(
                new AccountCreateOptions()
                {
                    Type = "express",
                    BusinessType = "company",
                    Company = new AccountCompanyOptions()
                    {
                        Name = restaurant.Name,
                    },
                });

            return account.Id;
        }

        public async Task<string> GenerateOnboardingLink(
            BillingAccountId id, RestaurantId restaurantId)
        {
            var service = new AccountLinkService();

            var link = await service.CreateAsync(
                new AccountLinkCreateOptions()
                {
                    Account = id,
                    RefreshUrl = $"{config.StripeOnboardingRefreshUrl}?restaurant_id={restaurantId.Value}",
                    ReturnUrl = config.StripeOnboardingReturnUrl,
                    Type = "account_onboarding",
                });

            return link.Url;
        }
    }
}
