using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Features.Billing;

namespace WebTests.Doubles
{
    public class BillingServiceSpy : IBillingService
    {
        public string AccountId { get; set; }
        public string OnboardingLink { get; set; }

        public Task<string> CreateAccount(Restaurant restaurant)
        {
            return Task.FromResult(AccountId);
        }

        public Task<string> GenerateOnboardingLink(
            BillingAccountId id, RestaurantId restaurantId)
        {
            return Task.FromResult(OnboardingLink);
        }
    }
}
