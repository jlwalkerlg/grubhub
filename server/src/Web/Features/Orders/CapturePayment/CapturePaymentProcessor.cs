using System.Threading;
using System.Threading.Tasks;
using Stripe;
using Web.Services.Jobs;

namespace Web.Features.Orders.CapturePayment
{
    public class CapturePaymentProcessor : IJobProcessor<CapturePaymentJob>
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
