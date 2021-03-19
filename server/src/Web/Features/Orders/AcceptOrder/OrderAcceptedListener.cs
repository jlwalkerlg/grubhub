using System.Threading;
using System.Threading.Tasks;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.AcceptOrder
{
    public class OrderAcceptedListener : IEventListener<OrderAcceptedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJobQueue queue;

        public OrderAcceptedListener(IUnitOfWork unitOfWork, IJobQueue queue)
        {
            this.unitOfWork = unitOfWork;
            this.queue = queue;
        }

        public async Task Handle(OrderAcceptedEvent evnt, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(evnt.OrderId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await queue.Enqueue(new Job[]
            {
                new NotifyUserOrderAcceptedJob(order.Id.Value, order.UserId.Value.ToString()),
                new NotifyRestaurantOrderAcceptedJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
            }, cancellationToken);
        }
    }
}
