using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services.Events;

namespace WebTests.Doubles
{
    public class EventBusSpy : IEventBus
    {
        public List<Event> Events { get; } = new();

        public Task Publish(Event ev)
        {
            Events.Add(ev);
            return Task.CompletedTask;
        }
    }
}
