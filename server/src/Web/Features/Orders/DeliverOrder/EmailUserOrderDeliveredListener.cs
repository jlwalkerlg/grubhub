using System.Threading.Tasks;
using DotNetCore.CAP;
using Web.Services.Events;
using Web.Services.Mail;

namespace Web.Features.Orders.DeliverOrder
{
    [CapSubscribe(nameof(EmailUserOrderDeliveredListener))]
    public class EmailUserOrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailer mailer;
        private readonly MailSettings settings;

        public EmailUserOrderDeliveredListener(IUnitOfWork unitOfWork, IMailer mailer, MailSettings settings)
        {
            this.unitOfWork = unitOfWork;
            this.mailer = mailer;
            this.settings = settings;
        }

        [CapSubscribe(nameof(OrderDeliveredEvent), isPartial: true)]
        public async Task Handle(OrderDeliveredEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(@event.OrderId);
            var user = await unitOfWork.Users.GetById(order.UserId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await mailer.Send(new Mail(settings.FromAddress, user.Email.Address)
            {
                ToName = user.Name,
                FromName = settings.FromName,
                Subject = "Order delivered!",
                Body = $"{restaurant.Name} delivered your order at {order.DeliveredAt?.ToString("h:mm:ss tt")}. The total cost was £{order.CalculateTotal().Pounds}.",
            });
        }
    }
}
