using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Web.Features.Billing.UpdateBillingDetails;
using Web.Features.Orders.ConfirmOrder;
using Web.Services.Antiforgery;

namespace Web.Webhooks
{
    public class StripeWebhooksAction : Action
    {
        private readonly StripeSettings settings;
        private readonly ILogger<StripeWebhooksAction> logger;
        private readonly ISender sender;

        public StripeWebhooksAction(
            StripeSettings settings,
            ILogger<StripeWebhooksAction> logger,
            ISender sender)
        {
            this.settings = settings;
            this.logger = logger;
            this.sender = sender;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/stripe/webhooks")]
        public Task<IActionResult> Webhook() =>
            Execute(settings.WebhookSigningSecret);

        [IgnoreAntiforgeryValidation]
        [HttpPost("/stripe/connect/webhooks")]
        public Task<IActionResult> ConnectWebhook() =>
            Execute(settings.ConnectWebhookSigningSecret);

        private async Task<IActionResult> Execute(string webhookSigningSecret)
        {
            try
            {
                var stripeEvent = await DecodeStripeEvent(webhookSigningSecret);

                logger.LogInformation("Stripe event: " + stripeEvent.Type);

                var result = stripeEvent.Type switch
                {
                    Events.AccountUpdated => await HandleAccountUpdate(
                        stripeEvent.Data.Object as Account),

                    Events.PaymentIntentAmountCapturableUpdated => await HandlePaymentConfirmed(
                        stripeEvent.Data.Object as PaymentIntent),

                    _ => Result.Ok(),
                };

                if (result) return Ok();

                logger.LogError(result.Error);
                return Problem(result.Error);
            }
            catch (StripeException e)
            {
                logger.LogError(e.ToString());
                return BadRequest();
            }
        }

        private async Task<Event> DecodeStripeEvent(string webhookSigningSecret)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            return EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                webhookSigningSecret);
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

        private async Task<Result> HandlePaymentConfirmed(PaymentIntent intent)
        {
            if (intent.Status != "requires_capture")
            {
                return Result.Ok();
            }

            var command = new ConfirmOrderByPaymentIntentIdCommand()
            {
                PaymentIntentId = intent.Id,
            };

            return await sender.Send(command);
        }
    }
}
