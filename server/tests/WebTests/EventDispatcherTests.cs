using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Data.EF;
using Web.Domain.Orders;
using Web.Features.Events;
using Web.Features.Orders.ConfirmOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests
{
    public class EventDispatcherTests
    {
        private readonly SenderSpy sender;
        private readonly EventDispatcher dispatcher;

        public EventDispatcherTests()
        {
            sender = new SenderSpy();

            dispatcher = new EventDispatcher(sender);
        }

        [Fact]
        public async Task It_Dispatches_OrderConfirmedEvents()
        {
            var ev = new OrderConfirmedEvent(
                new OrderId(Guid.NewGuid().ToString()),
                DateTime.UtcNow);

            var evDto = new EventDto(ev);

            sender.Response = Result.Ok();

            await dispatcher.Dispatch(evDto, default);

            var command = sender.Requests
                .OfType<HandleEventCommand<OrderConfirmedEvent>>()
                .Single();

            command.Event.ShouldBe(ev);
        }
    }
}
