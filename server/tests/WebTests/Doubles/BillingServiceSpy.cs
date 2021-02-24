using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Features.Billing;

namespace WebTests.Doubles
{
    public class BillingServiceSpy : IBillingService
    {
        public string AccountId { get; set; }
        public string OnboardingLink { get; set; }

        public Result<PaymentIntent> PaymentIntentResult { get; set; }

        public Money PaymentIntentAmount { get; private set; }
        public BillingAccount PaymentIntentAccount { get; private set; }

        public bool PaymentAccepted { get; set; }
        public Order ConfirmedOrder { get; private set; }

        public Task<bool> CheckPaymentWasAccepted(Order order)
        {
            ConfirmedOrder = order;
            return Task.FromResult(PaymentAccepted);
        }

        public Task<string> CreateAccount(Restaurant restaurant)
        {
            return Task.FromResult(AccountId);
        }

        public Task<string> GenerateOnboardingLink(
            BillingAccountId id, RestaurantId restaurantId)
        {
            return Task.FromResult(OnboardingLink);
        }

        public Task<Result<PaymentIntent>> GeneratePaymentIntent(
            Order order, BillingAccount account)
        {
            PaymentIntentAmount = order.CalculateTotal();
            PaymentIntentAccount = account;
            return Task.FromResult(PaymentIntentResult);
        }
    }
}
