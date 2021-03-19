using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyUserOrderCancelledProcessor : IJobProcessor<NotifyUserOrderCancelledJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderCancelledProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyUserOrderCancelledJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.UserId)
                .SendAsync($"order_{job.OrderId}.cancelled", cancellationToken);

            return Result.Ok();
        }
    }
}
