using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyUserOrderRejectedProcessor : IJobProcessor<NotifyUserOrderRejectedJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderRejectedProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyUserOrderRejectedJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.UserId)
                .SendAsync($"order_{job.OrderId}.rejected", cancellationToken);

            return Result.Ok();
        }
    }
}
