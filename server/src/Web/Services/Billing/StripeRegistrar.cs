using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Web.Features.Billing;

namespace Web.Services.Billing
{
    public static class StripeRegistrar
    {
        public static void AddStripe(this IServiceCollection services, StripeSettings settings)
        {
            StripeConfiguration.ApiKey = settings.SecretKey;

            services.AddSingleton<IBillingService, StripeBillingService>();
        }
    }
}
