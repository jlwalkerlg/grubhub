using System.Collections.Generic;
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
                        Destination = account.Id.Value,
                    },
                },
                new RequestOptions()
                {
                    IdempotencyKey = order.Id.Value,
                }
            );

            return Result.Ok(
                new Web.Features.Billing.PaymentIntent()
                {
                    Id = intent.Id,
                    ClientSecret = intent.ClientSecret,
                });
        }

        public async Task<bool> CheckPaymentWasAccepted(Domain.Orders.Order order)
        {
            var service = new PaymentIntentService();

            try
            {
                var intent = await service.GetAsync(order.PaymentIntentId);

                switch (intent.Status)
                {
                    case "requires_capture":
                    case "canceled":
                    case "succeeded":
                        return true;
                    default:
                        return false;
                }
            }
            catch (StripeException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
