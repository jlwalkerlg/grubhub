using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyRestaurantOrderConfirmedListener : IEventListener<OrderConfirmedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderConfirmedListener(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public async Task Handle(OrderConfirmedEvent @event, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(@event.OrderId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await hubContext
                .Clients
                .Users(restaurant.ManagerId.Value.ToString())
                .SendAsync("new-order", order.Id.Value, cancellationToken);
        }
    }
}
