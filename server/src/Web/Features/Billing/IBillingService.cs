using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Orders;
using Web.Domain.Restaurants;

namespace Web.Features.Billing
{
    public interface IBillingService
    {
        public Task<string> CreateAccount(Restaurant restaurant);
        Task<string> GenerateOnboardingLink(BillingAccountId id, RestaurantId restaurantId);
        Task<Result<PaymentIntent>> GeneratePaymentIntent(Money amount, BillingAccount account);
        Task<Result> ConfirmOrder(Order order);
    }
}
