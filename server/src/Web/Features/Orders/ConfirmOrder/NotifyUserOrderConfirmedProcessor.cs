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

        public async Task<Result> Handle(
            NotifyUserOrderConfirmedJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(job.OrderId);

            if (order is null) return Error.NotFound("Order not found.");

            var user = await unitOfWork.Users.GetById(order.UserId);

            if (user is null) return Error.NotFound("User not found.");

            await notifier.NotifyCustomerOrderConfirmed(user, order);

            return Result.Ok();
        }
    }
}
