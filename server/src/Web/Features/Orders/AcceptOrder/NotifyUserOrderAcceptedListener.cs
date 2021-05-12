using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyUserOrderAcceptedListener : IEventListener<OrderAcceptedEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hub;

        public NotifyUserOrderAcceptedListener(IUnitOfWork uow, IHubContext<OrderHub> hub)
        {
            this.uow = uow;
            this.hub = hub;
        }

        [CapSubscribe(nameof(OrderAcceptedEvent), Group = nameof(NotifyUserOrderAcceptedListener))]
        public async Task Handle(OrderAcceptedEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await hub.Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.accepted");
        }
    }
}
