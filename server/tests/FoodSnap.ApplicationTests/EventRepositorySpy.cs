using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.ApplicationTests
{
    public class EventRepositorySpy : IEventRepository
    {
        public List<IEvent> Events = new List<IEvent>();

        public Task Add<TEvent>(TEvent ev) where TEvent : IEvent
        {
            Events.Add(ev);
            return Task.CompletedTask;
        }
    }
}
