using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Jobs;
using Web.Services.Mail;

namespace Web.Features.Orders.DeliverOrder
{
    public class EmailUserOrderDeliveredProcessor : IJobProcessor<EmailUserOrderDeliveredJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailer mailer;
        private readonly MailSettings settings;

        public EmailUserOrderDeliveredProcessor(IUnitOfWork unitOfWork, IMailer mailer, MailSettings settings)
        {
            this.unitOfWork = unitOfWork;
            this.mailer = mailer;
            this.settings = settings;
        }

        public async Task<Result> Handle(EmailUserOrderDeliveredJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(job.OrderId));
            var user = await unitOfWork.Users.GetById(order.UserId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await mailer.Send(new Mail(settings.FromAddress, user.Email.Address)
            {
                ToName = user.Name,
                FromName = settings.FromName,
                Subject = "Order delivered!",
                Body = $"{restaurant.Name} delivered your order at {order.DeliveredAt?.ToString("h:mm:ss tt")}. The total cost was £{order.CalculateTotal().Pounds}.",
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
