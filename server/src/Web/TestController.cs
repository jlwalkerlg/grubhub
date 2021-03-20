using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Services.Events;

namespace Web
{
    public class TestController : Controller
    {
        private readonly IUnitOfWork uow;

        public TestController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("/test")]
        public async Task Test()
        {
            await uow.Events.Store(new DummyEvent());
            await uow.Commit();
        }
    }

    public record DummyEvent() : Event(DateTime.Now);

    public class DummyEventListener : IEventListener<DummyEvent>
    {
        public Task Handle(DummyEvent @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("Dummy event listener.");
            return Task.CompletedTask;
        }
    }

    public class AnotherDummyEventListener : IEventListener<DummyEvent>
    {
        public Task Handle(DummyEvent @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("Another dummy event listener.");
            return Task.CompletedTask;
        }
    }
}
