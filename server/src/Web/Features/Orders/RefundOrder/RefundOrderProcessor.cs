using System.Threading;
using System.Threading.Tasks;
using Stripe;
using Web.Services.Jobs;

namespace Web.Features.Orders.RefundOrder
{
    public class RefundOrderProcessor : IJobProcessor<RefundOrderJob>
    {
        private readonly PaymentIntentService service = new();

        public async Task<Result> Handle(RefundOrderJob job, CancellationToken cancellationToken)
        {
            await service.CancelAsync(
                job.PaymentIntentId,
                new PaymentIntentCancelOptions(),
                cancellationToken: cancellationToken);

            return Result.Ok();
        }
    }
}
