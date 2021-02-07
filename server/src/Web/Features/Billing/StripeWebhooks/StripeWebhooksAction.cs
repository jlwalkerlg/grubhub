using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Web.Features.Billing.UpdateBillingDetails;

namespace Web.Features.Billing.EnableBilling
{
    public class StripeWebhooksAction : Action
    {
        private readonly Config config;
        private readonly ILogger<StripeWebhooksAction> logger;
        private readonly ISender sender;

        public StripeWebhooksAction(
            Config config,
            ILogger<StripeWebhooksAction> logger,
            ISender sender)
        {
            this.config = config;
            this.logger = logger;
            this.sender = sender;
        }

        [HttpPost("/stripe/webhooks")]
        public async Task<IActionResult> Execute()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    config.StripeWebhookSigningSecret);

                logger.LogInformation("Stripe event: " + stripeEvent.Type);

                var result = Result.Ok();

                if (stripeEvent.Type == Stripe.Events.AccountUpdated)
                {
                    var account = stripeEvent.Data.Object as Account;
                    result = await HandleAccountUpdate(account);
                }

                if (!result)
                {
                    logger.LogError(result.Error);
                    return Error(result.Error);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                logger.LogError(e.ToString());
                return BadRequest();
            }
        }

        private async Task<Result> HandleAccountUpdate(Account account)
        {
            var command = new UpdateBillingDetailsCommand()
            {
                BillingAccountId = account.Id,
                IsBillingEnabled = account.ChargesEnabled,
            };

            return await sender.Send(command);
        }
    }
}
