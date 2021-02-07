using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;

namespace Web.Features.Billing
{
    public interface IBillingService
    {
        public Task<string> CreateAccount(Restaurant restaurant);
        Task<string> GenerateOnboardingLink(BillingAccountId id, RestaurantId restaurantId);
    }
}
