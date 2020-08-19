using System;
using FoodSnap.Application.Events;
using Newtonsoft.Json;

namespace FoodSnap.Infrastructure.Persistence.EF
{
    public class EventDto
    {
        public long Id { get; }
        public string EventType { get; }
        public string Data { get; }
        public DateTime CreatedAt { get; }

        public EventDto(Event ev)
        {
            EventType = ev.GetType().ToString();
            Data = JsonConvert.SerializeObject(ev);
            CreatedAt = ev.CreatedAt;
        }

        public TEvent ToEvent<TEvent>() where TEvent : Event
        {
            return JsonConvert.DeserializeObject<TEvent>(Data);
        }

        private EventDto() { }
    }
}
