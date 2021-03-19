using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;

namespace WebTests.Doubles
{
    public class EventStoreSpy : IEventStore
    {
        public List<Event> Events { get; } = new();

        public Task Store(Event @event)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }
}
