using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Domain.Orders;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyRestaurantOrderRejectedListener : IEventListener<OrderRejectedEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderRejectedListener(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            this.uow = uow;
            this.hubContext = hubContext;
        }

        [CapSubscribe(nameof(OrderRejectedEvent), Group = nameof(NotifyRestaurantOrderRejectedListener))]
        public async Task Handle(OrderRejectedEvent @event)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));
            var restaurant = await uow.Restaurants.GetById(order.RestaurantId);

            await hubContext.Clients
                .Users(restaurant.ManagerId.Value.ToString())
                .SendAsync("order-rejected", order.Id.Value);
        }
    }
}
