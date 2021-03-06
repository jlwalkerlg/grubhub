using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyCustomerOrderCancelledProcessor : JobProcessor<NotifyCustomerOrderCancelledJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyCustomerOrderCancelledProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyCustomerOrderCancelledJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.CustomerId)
                .SendAsync($"order_{job.OrderId}.cancelled", cancellationToken);

            return Result.Ok();
        }
    }
}
