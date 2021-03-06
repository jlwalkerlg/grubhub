using System.Threading;
using System.Threading.Tasks;

namespace Web.Features.Orders
{
    public class RefundOrderProcessor : JobProcessor<RefundOrderJob>
    {
        public Task<Result> Handle(RefundOrderJob request, CancellationToken cancellationToken)
        {
            // TODO
            return Task.FromResult(Result.Ok());
        }
    }
}
