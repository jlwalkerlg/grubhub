using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyUserOrderConfirmedProcessor : JobProcessor<NotifyUserOrderConfirmedJob>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderConfirmedProcessor(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public async Task<Result> Handle(
            NotifyUserOrderConfirmedJob job, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(job.OrderId);

            if (order is null) return Error.NotFound("Order not found.");

            await hubContext
                .Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.confirmed", cancellationToken);

            return Result.Ok();
        }
    }
}
