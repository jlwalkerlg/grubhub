using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Web.Features.Billing;

namespace Web.Services.Billing
{
    public static class StripeRegistrar
    {
        public static void AddStripe(this IServiceCollection services, Config config)
        {
            StripeConfiguration.ApiKey = config.StripeSecretKey;

            services.AddSingleton<IBillingService, StripeBillingService>();
        }
    }
}
