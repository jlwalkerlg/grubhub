using System;
using FoodSnap.Application;

namespace FoodSnap.Infrastructure.Persistence.EF
{
    public class EventDto
    {
        public long Id { get; }
        public string EventType { get; }
        public Event Event { get; }
        public DateTime CreatedAt { get; }

        public EventDto(Event ev)
        {
            EventType = ev.GetType().ToString();
            Event = ev;
            CreatedAt = ev.CreatedAt;
        }

        private EventDto() { }
    }
}
