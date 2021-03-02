using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyCustomerOrderDeliveredProcessor : JobProcessor<NotifyCustomerOrderDeliveredJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyCustomerOrderDeliveredProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyCustomerOrderDeliveredJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.CustomerId)
                .SendAsync($"order_{job.OrderId}.delivered", cancellationToken);

            return Result.Ok();
        }
    }
}
