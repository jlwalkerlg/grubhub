using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<Result<Web.Features.Billing.PaymentIntent>> GeneratePaymentIntent(
            Web.Domain.Orders.Order order, BillingAccount account)
        {
            var service = new PaymentIntentService();

            var intent = await service.CreateAsync(
                new PaymentIntentCreateOptions()
                {
                    CaptureMethod = "manual",
                    PaymentMethodTypes = new List<string>() { "card" },
                    Amount = order.CalculateTotal().Pence,
                    Currency = "gbp",
                    ApplicationFeeAmount = 50,
                    TransferData = new PaymentIntentTransferDataOptions()
                    {
                        Destination = account.Id.Value.ToString(),
                    },
                },
                new RequestOptions()
                {
                    IdempotencyKey = order.Id.Value.ToString(),
                }
            );

            return Result.Ok(
                new Web.Features.Billing.PaymentIntent()
                {
                    Id = intent.Id,
                    ClientSecret = intent.ClientSecret,
                });
        }

        public async Task<Result> EnsurePaymentWasAccepted(Domain.Orders.Order order)
        {
            var service = new PaymentIntentService();

            var whitelist = new[]
            {
                "requires_capture",
                "canceled",
                "succeeded",
            };

            try
            {
                var intent = await service.GetAsync(order.PaymentIntentId);

                if (whitelist.Contains(intent.Status))
                {
                    return Result.Ok();
                }

                return Error.BadRequest("Payment not confirmed.");
            }
            catch (Stripe.StripeException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return Web.Error.NotFound("Payment details not found.");
                }

                return Web.Error.BadRequest("Failed to confirm order.");
            }
        }
    }
}
