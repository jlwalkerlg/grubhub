using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Web.Features.Orders
{
    public class CapturePaymentProcessor : JobProcessor<CapturePaymentJob>
    {
        private readonly PaymentIntentService service = new();

        public async Task<Result> Handle(CapturePaymentJob job, CancellationToken cancellationToken)
        {
            await service.CaptureAsync(
                job.PaymentIntentId,
                new PaymentIntentCaptureOptions(),
                cancellationToken: cancellationToken);

            return Result.Ok();
        }
    }
}
