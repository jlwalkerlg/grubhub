using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Events;

namespace WebTests.Doubles
{
    public class EventRepositorySpy : IEventRepository
    {
        public List<Event> Events = new();

        public Task Add<TEvent>(TEvent ev) where TEvent : Event
        {
            Events.Add(ev);
            return Task.CompletedTask;
        }

        public Task<List<Event>> All()
        {
            return Task.FromResult(Events);
        }
    }
}