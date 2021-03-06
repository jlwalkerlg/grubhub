using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyRestaurantOrderCancelledProcessor : JobProcessor<NotifyRestaurantOrderCancelledJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderCancelledProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyRestaurantOrderCancelledJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.ManagerId)
                .SendAsync("order-cancelled", job.OrderId, cancellationToken);

            return Result.Ok();
        }
    }
}
