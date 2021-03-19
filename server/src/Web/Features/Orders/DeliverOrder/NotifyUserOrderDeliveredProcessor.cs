using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyUserOrderDeliveredProcessor : IJobProcessor<NotifyUserOrderDeliveredJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderDeliveredProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyUserOrderDeliveredJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.UserId)
                .SendAsync($"order_{job.OrderId}.delivered", cancellationToken);

            return Result.Ok();
        }
    }
}
