using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyRestaurantOrderCancelledListener : IEventListener<OrderCancelledEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderCancelledListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        public async Task Handle(OrderCancelledEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));
            var restaurant = await uow.Restaurants.GetById(order.RestaurantId);

            await hubContext.Clients
                .Users(restaurant.ManagerId.Value.ToString())
                .SendAsync("order-cancelled", order.Id.Value, cancellationToken);
        }
    }
}
