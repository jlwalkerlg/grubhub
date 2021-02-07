using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Web.Features.Billing;
using Web.Services.Billing;

namespace Web.ServiceRegistration
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
