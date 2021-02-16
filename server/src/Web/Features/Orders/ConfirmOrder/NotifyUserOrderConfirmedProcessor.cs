using System.Threading;
using System.Threading.Tasks;
using Web.Services.Notifications;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyUserOrderConfirmedProcessor : JobProcessor<NotifyUserOrderConfirmedJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotifier notifier;

        public NotifyUserOrderConfirmedProcessor(IUnitOfWork unitOfWork, INotifier notifier)
        {
            this.unitOfWork = unitOfWork;
            this.notifier = notifier;
        }

        protected override async Task Process(
            NotifyUserOrderConfirmedJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(job.OrderId);
            if (order is null) return;

            var user = await unitOfWork.Users.GetById(order.UserId);
            if (user is null) return;

            await notifier.NotifyCustomerOrderConfirmed(user, order);
        }
    }
}
