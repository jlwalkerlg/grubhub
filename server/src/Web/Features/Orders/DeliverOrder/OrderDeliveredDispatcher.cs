using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Orders.CapturePayment;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.DeliverOrder
{
    public class OrderDeliveredDispatcher : EventDispatcher<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderDeliveredDispatcher(IUnitOfWork unitOfWork, IJobQueue queue) : base(queue)
        {
            this.unitOfWork = unitOfWork;
        }

        protected override async Task<IEnumerable<Job>> GetJobs(OrderDeliveredEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(@event.OrderId);

            return new Job[]
            {
                new NotifyUserOrderDeliveredJob(
                    order.Id.Value,
                    order.UserId.Value.ToString()),
                new NotifyRestaurantOrderDeliveredJob(
                    order.Id.Value,
                    order.RestaurantId.Value.ToString()),
                new CapturePaymentJob(order.PaymentIntentId),
                new EmailUserOrderDeliveredJob(order.Id.Value),
            };
        }
    }
}
