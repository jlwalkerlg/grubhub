using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Features.Orders.ConfirmOrder;
using Web.Services.Notifications;

namespace Web.Features.Orders
{
    public class OrderConfirmedEventHandler : INotificationHandler<OrderConfirmedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotifier notifier;

        public OrderConfirmedEventHandler(IUnitOfWork unitOfWork, INotifier notifier)
        {
            this.unitOfWork = unitOfWork;
            this.notifier = notifier;
        }

        public async Task Handle(
            OrderConfirmedEvent ocEvent, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(ocEvent.OrderId);

            if (order is null) return;

            var user = await unitOfWork.Users.GetById(order.UserId);

            if (user is null) return;

            await notifier.NotifyOrderConfirmed(user, order);
        }
    }
}
