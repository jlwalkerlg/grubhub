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

        public Result ConfirmResult { get; set; }
        public Order ConfirmedOrder { get; private set; }

        public Task<Result> ConfirmOrder(Order order)
        {
            ConfirmedOrder = order;
            return Task.FromResult(ConfirmResult);
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

        public Task<Result<PaymentIntent>> GeneratePaymentIntent(Money amount, BillingAccount account)
        {
            PaymentIntentAmount = amount;
            PaymentIntentAccount = account;
            return Task.FromResult(PaymentIntentResult);
        }
    }
}
