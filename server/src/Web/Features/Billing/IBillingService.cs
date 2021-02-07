using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Features.Billing
{
    public interface IBillingService
    {
        public Task<string> CreateAccount(Restaurant restaurant, RestaurantManager manager);
        Task<string> GenerateOnboardingLink(BillingAccountId id, RestaurantId restaurantId);
    }
}
