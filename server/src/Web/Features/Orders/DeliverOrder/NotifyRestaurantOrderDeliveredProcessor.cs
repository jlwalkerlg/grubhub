﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Jobs;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyRestaurantOrderDeliveredProcessor : IJobProcessor<NotifyRestaurantOrderDeliveredJob>
    {
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderDeliveredProcessor(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(NotifyRestaurantOrderDeliveredJob job, CancellationToken cancellationToken)
        {
            await hubContext.Clients
                .Users(job.RestaurantId)
                .SendAsync("order-delivered", job.OrderId, cancellationToken);

            return Result.Ok();
        }
    }
}
