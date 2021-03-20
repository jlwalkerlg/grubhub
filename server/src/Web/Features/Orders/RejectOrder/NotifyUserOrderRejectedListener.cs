using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyUserOrderRejectedListener : IEventListener<OrderRejectedEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderRejectedListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        public async Task Handle(OrderRejectedEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));

            await hubContext.Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.rejected", cancellationToken);
        }
    }
}
