using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.ConfirmOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders
{
    public class OrderConfirmedListenerTests
    {
        private readonly JobQueueSpy jobQueueSpy;
        private readonly OrderConfirmedListener listener;

        public OrderConfirmedListenerTests()
        {
            jobQueueSpy = new JobQueueSpy();

            listener = new OrderConfirmedListener(jobQueueSpy);
        }

        [Fact]
        public async Task It_Adds_Jobs_To_The_Queue()
        {
            var ocEvent = new OrderConfirmedEvent(
                new OrderId(Guid.NewGuid().ToString()),
                DateTime.UtcNow);

            await listener.Handle(ocEvent, default);

            jobQueueSpy.Jobs
                .OfType<NotifyRestaurantOrderConfirmedJob>()
                .Single()
                .OrderId
                .ShouldBe(ocEvent.OrderId);

            jobQueueSpy.Jobs
                .OfType<NotifyUserOrderConfirmedJob>()
                .Single()
                .OrderId
                .ShouldBe(ocEvent.OrderId);
        }
    }
}
