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
    public class EventDispatcherTests
    {
        private readonly PublisherSpy publisherSpy;
        private readonly EventDispatcher dispatcher;

        public EventDispatcherTests()
        {
            publisherSpy = new PublisherSpy();

            dispatcher = new EventDispatcher(publisherSpy);
        }

        [Fact]
        public async Task It_Dispatches_OrderConfirmedEvents()
        {
            var ocEvent = new OrderConfirmedEvent(
                new OrderId(Guid.NewGuid().ToString()),
                DateTime.UtcNow);

            var e = new EventDto(ocEvent);

            await dispatcher.Dispatch(e, default);

            e.Handled.ShouldBeTrue();

            var oce = publisherSpy.Notifications.Single() as OrderConfirmedEvent;

            oce.ShouldBe(ocEvent);
        }
    }
}
