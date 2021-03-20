using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.AcceptOrder
{
    public class OrderAcceptedDispatcher : EventDispatcher<OrderAcceptedEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderAcceptedDispatcher(IJobQueue queue, IUnitOfWork unitOfWork) : base(queue)
        {
            this.unitOfWork = unitOfWork;
        }

        protected override async Task<IEnumerable<Job>> GetJobs(OrderAcceptedEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(@event.OrderId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            return new Job[]
            {
                new NotifyUserOrderAcceptedJob(order.Id.Value, order.UserId.Value.ToString()),
                new NotifyRestaurantOrderAcceptedJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
            };
        }
    }
}
