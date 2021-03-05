using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.ConfirmOrder;
using Web.Services.Events;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders
{
    public class OrderConfirmedListenerTests
    {
        private readonly JobQueueSpy queue;
        private readonly OrderConfirmedListener listener;

        public OrderConfirmedListenerTests()
        {
            queue = new JobQueueSpy();

            listener = new OrderConfirmedListener(queue);
        }

        [Fact]
        public async Task It_Adds_Jobs_To_The_Queue()
        {
            var ev = new OrderConfirmedEvent(
                new OrderId(Guid.NewGuid().ToString()),
                DateTime.UtcNow);

            await listener.Handle(ev, default);

            queue.Jobs
                .OfType<NotifyRestaurantOrderConfirmedJob>()
                .Single()
                .OrderId
                .ShouldBe(ev.OrderId);

            queue.Jobs
                .OfType<NotifyUserOrderConfirmedJob>()
                .Single()
                .OrderId
                .ShouldBe(ev.OrderId);
        }
    }
}
