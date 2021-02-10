using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Stripe;
using Web.Domain;
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
            Money amount, BillingAccount account)
        {
            var service = new PaymentIntentService();

            var amountInPence = (int)(amount.Amount * 100);
            var applicationFeeInPence = 50;

            var intent = await service.CreateAsync(
                new PaymentIntentCreateOptions()
                {
                    CaptureMethod = "manual",
                    PaymentMethodTypes = new List<string>() { "card" },
                    Amount = amountInPence,
                    Currency = "gbp",
                    ApplicationFeeAmount = applicationFeeInPence,
                    TransferData = new PaymentIntentTransferDataOptions()
                    {
                        Destination = account.Id.Value.ToString(),
                    },
                }
            );

            return Result.Ok(
                new Web.Features.Billing.PaymentIntent()
                {
                    Id = intent.Id,
                    ClientSecret = intent.ClientSecret,
                });
        }

        public async Task<Result> ConfirmOrder(Domain.Orders.Order order)
        {
            var service = new PaymentIntentService();

            try
            {
                var intent = await service.GetAsync(order.PaymentIntentId);

                if (intent.Status == "requires_capture")
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
