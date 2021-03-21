using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;

namespace WebTests.Doubles
{
    public class OutboxSpy : IOutbox
    {
        public List<Event> Events { get; } = new();

        public Task Add(Event @event)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }
}
