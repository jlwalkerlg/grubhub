using System.Threading;
using System.Threading.Tasks;
using Web.Services.Notifications;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyRestaurantOrderConfirmedProcessor : JobProcessor<NotifyRestaurantOrderConfirmedJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotifier notifier;

        public NotifyRestaurantOrderConfirmedProcessor(
            IUnitOfWork unitOfWork, INotifier notifier)
        {
            this.unitOfWork = unitOfWork;
            this.notifier = notifier;
        }

        protected override async Task Process(
            NotifyRestaurantOrderConfirmedJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(job.OrderId);
            if (order is null) return;

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);
            if (restaurant is null) return;

            var manager = await unitOfWork.Users.GetManagerById(restaurant.ManagerId);
            if (manager is null) return;

            await notifier.NotifyRestaurantOrderConfirmed(manager, order);
        }
    }
}
