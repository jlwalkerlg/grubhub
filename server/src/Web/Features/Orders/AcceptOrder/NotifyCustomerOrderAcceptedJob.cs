using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyCustomerOrderAcceptedJob : Job
    {
        public NotifyCustomerOrderAcceptedJob(string orderId, string customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }

        public string OrderId { get; }
        public string CustomerId { get; }
    }

    public class NotifyCustomerOrderAcceptedProcessor : JobProcessor<NotifyCustomerOrderAcceptedJob>
    {
        private readonly IHubContext<OrderHub> hub;

        public NotifyCustomerOrderAcceptedProcessor(IHubContext<OrderHub> hub)
        {
            this.hub = hub;
        }

        public async Task<Result> Handle(NotifyCustomerOrderAcceptedJob job, CancellationToken cancellationToken)
        {
            await hub.Clients
                .Users(job.CustomerId)
                .SendAsync($"order_{job.OrderId}.accepted", cancellationToken);

            return Result.Ok();
        }
    }
}
