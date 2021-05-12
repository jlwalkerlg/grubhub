using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyUserOrderConfirmedListener : IEventListener<OrderConfirmedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<OrderHub> hubContext;

        public NotifyUserOrderConfirmedListener(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        [CapSubscribe(nameof(OrderConfirmedEvent), Group = nameof(NotifyUserOrderConfirmedListener))]
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
