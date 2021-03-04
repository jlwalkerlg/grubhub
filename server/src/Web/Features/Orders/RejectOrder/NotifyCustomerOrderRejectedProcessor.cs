using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyCustomerOrderRejectedProcessor : JobProcessor<NotifyCustomerOrderRejectedJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyCustomerOrderRejectedProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyCustomerOrderRejectedJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.CustomerId)
                .SendAsync($"order_{job.OrderId}.rejected", cancellationToken);

            return Result.Ok();
        }
    }
}
