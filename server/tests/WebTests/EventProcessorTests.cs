using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Data.EF;
using Web.Domain.Orders;
using Web.Features.Orders.ConfirmOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests
{
    public class EventProcessorTests
    {
        private readonly PublisherSpy publisherSpy;
        private readonly EventProcessor processor;

        public EventProcessorTests()
        {
            publisherSpy = new PublisherSpy();

            processor = new EventProcessor(publisherSpy);
        }

        [Fact]
        public async Task It_Processes_OrderConfirmedEvents()
        {
            var ocEvent = new OrderConfirmedEvent(
                new OrderId(Guid.NewGuid().ToString()),
                DateTime.UtcNow);

            var e = new EventDto(ocEvent);

            await processor.ProcessEvent(e);

            e.Handled.ShouldBeTrue();

            publisherSpy.PublishedNotifications.ShouldHaveSingleItem();

            var notification = publisherSpy.PublishedNotifications.Single();
            notification.ShouldBeOfType<OrderConfirmedEvent>();

            var oceNotification = notification as OrderConfirmedEvent;
            oceNotification.ShouldBe(ocEvent);
        }
    }
}
