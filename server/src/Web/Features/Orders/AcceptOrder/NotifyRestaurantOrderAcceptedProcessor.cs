﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyRestaurantOrderAcceptedProcessor : JobProcessor<NotifyRestaurantOrderAcceptedJob>
    {
        private readonly IHubContext<OrderHub> hub;

        public NotifyRestaurantOrderAcceptedProcessor(IHubContext<OrderHub> hub)
        {
            this.hub = hub;
        }

        public async Task<Result> Handle(NotifyRestaurantOrderAcceptedJob job, CancellationToken cancellationToken)
        {
            await hub.Clients
                .Users(job.ManagerId)
                .SendAsync("order-accepted", job.OrderId, cancellationToken);

            return Result.Ok();
        }
    }
}