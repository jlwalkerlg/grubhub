using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.ConfirmOrder
{
    public class OrderConfirmedDispatcher : EventDispatcher<OrderConfirmedEvent>
    {
        public OrderConfirmedDispatcher(IJobQueue queue) : base(queue)
        {
        }

        protected override async Task<IEnumerable<Job>> GetJobs(OrderConfirmedEvent @event)
        {
            await Task.CompletedTask;

            return new Job[]
            {
                new NotifyUserOrderConfirmedJob(@event.OrderId),
                new NotifyRestaurantOrderConfirmedJob(@event.OrderId),
            };
        }
    }
}
