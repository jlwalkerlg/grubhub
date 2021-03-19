using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyRestaurantOrderRejectedProcessor : IJobProcessor<NotifyRestaurantOrderRejectedJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderRejectedProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyRestaurantOrderRejectedJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.ManagerId)
                .SendAsync("order-rejected", job.OrderId, cancellationToken);

            return Result.Ok();
        }
    }
}
