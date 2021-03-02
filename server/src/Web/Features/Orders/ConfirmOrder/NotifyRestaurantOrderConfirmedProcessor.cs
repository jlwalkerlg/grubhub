using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyRestaurantOrderConfirmedProcessor : JobProcessor<NotifyRestaurantOrderConfirmedJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyRestaurantOrderConfirmedProcessor(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(
            NotifyRestaurantOrderConfirmedJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(job.OrderId);

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            await hubContext
                .Clients
                .Users(restaurant.ManagerId.Value.ToString())
                .SendAsync("new-order", order.Id.Value, cancellationToken);

            return Result.Ok();
        }
    }
}
