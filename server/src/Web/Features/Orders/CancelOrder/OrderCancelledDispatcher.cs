using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Orders.RefundOrder;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.CancelOrder
{
    public class OrderCancelledDispatcher : EventDispatcher<OrderCancelledEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderCancelledDispatcher(IUnitOfWork unitOfWork, IJobQueue queue) : base(queue)
        {
            this.unitOfWork = unitOfWork;
        }

        protected override async Task<IEnumerable<Job>> GetJobs(OrderCancelledEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(@event.OrderId));
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            return new Job[]
            {
                new NotifyUserOrderCancelledJob(order.Id.Value, order.UserId.Value.ToString()),
                new NotifyRestaurantOrderCancelledJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
                new RefundOrderJob(order.PaymentIntentId),
            };
        }
    }
}
