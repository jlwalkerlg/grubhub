using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Mail;

namespace Web.Features.Orders.DeliverOrder
{
    public class EmailUserOrderDeliveredProcessor : JobProcessor<EmailUserOrderDeliveredJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailer mailer;
        private readonly Config config;

        public EmailUserOrderDeliveredProcessor(IUnitOfWork unitOfWork, IMailer mailer, Config config)
        {
            this.unitOfWork = unitOfWork;
            this.mailer = mailer;
            this.config = config;
        }

        public async Task<Result> Handle(EmailUserOrderDeliveredJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(job.OrderId));
            var user = await unitOfWork.Users.GetById(order.UserId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await mailer.Send(new Mail(config.MailFromAddress, user.Email.Address)
            {
                ToName = user.Name,
                FromName = config.MailFromName,
                Subject = "Order delivered!",
                Body = $"{restaurant.Name} delivered your order at {order.DeliveredAt?.ToString("h:mm:ss tt")}. The total cost was £{order.CalculateTotal().Pounds}.",
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
