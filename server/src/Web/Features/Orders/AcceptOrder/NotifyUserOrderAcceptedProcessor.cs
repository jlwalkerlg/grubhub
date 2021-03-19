using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyUserOrderAcceptedProcessor : IJobProcessor<NotifyUserOrderAcceptedJob>
    {
        private readonly IHubContext<OrderHub> hub;

        public NotifyUserOrderAcceptedProcessor(IHubContext<OrderHub> hub)
        {
            this.hub = hub;
        }

        public async Task<Result> Handle(NotifyUserOrderAcceptedJob job, CancellationToken cancellationToken)
        {
            await hub.Clients
                .Users(job.UserId)
                .SendAsync($"order_{job.OrderId}.accepted", cancellationToken);

            return Result.Ok();
        }
    }
}
