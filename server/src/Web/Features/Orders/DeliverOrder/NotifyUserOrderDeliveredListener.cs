﻿using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyUserOrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderDeliveredListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        [Subscribe(nameof(OrderDeliveredEvent), typeof(NotifyUserOrderDeliveredListener))]
        public async Task Handle(OrderDeliveredEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await hubContext.Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.delivered");
        }
    }
}
