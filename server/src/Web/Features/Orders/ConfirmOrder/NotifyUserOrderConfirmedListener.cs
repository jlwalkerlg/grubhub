using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    [CapSubscribe(nameof(NotifyUserOrderConfirmedListener))]
    public class NotifyUserOrderConfirmedListener : IEventListener<OrderConfirmedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderConfirmedListener(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        [CapSubscribe(nameof(OrderConfirmedEvent), isPartial: true)]
        public async Task Handle(OrderConfirmedEvent @event)
        {
            var order = await unitOfWork.Orders.GetById(@event.OrderId);

            await hubContext
                .Clients
                .Users(order.UserId.Value.ToString())
                .SendAsync($"order_{order.Id.Value}.confirmed");
        }
    }
}
