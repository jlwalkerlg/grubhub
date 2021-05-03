using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    [CapSubscribe(nameof(NotifyUserOrderRejectedListener))]
    public class NotifyUserOrderRejectedListener : IEventListener<OrderRejectedEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderRejectedListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        [CapSubscribe(nameof(OrderRejectedEvent), isPartial: true)]
        public async Task Handle(OrderRejectedEvent @event)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));

            await hubContext.Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.rejected");
        }
    }
}
