using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Orders.RefundOrder;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.RejectOrder
{
    public class OrderRejectedDispatcher : EventDispatcher<OrderRejectedEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderRejectedDispatcher(IUnitOfWork unitOfWork, IJobQueue queue) : base(queue)
        {
            this.unitOfWork = unitOfWork;
        }

        protected override async Task<IEnumerable<Job>> GetJobs(OrderRejectedEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(@event.OrderId));
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            return new Job[]
            {
                new NotifyUserOrderRejectedJob(order.Id.Value, order.UserId.Value.ToString()),
                new NotifyRestaurantOrderRejectedJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
                new RefundOrderJob(order.PaymentIntentId),
            };
        }
    }
}
