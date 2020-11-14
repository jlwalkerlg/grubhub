using System;
using System.Text.Json;
using FoodSnap.Application.Events;

namespace FoodSnap.Infrastructure.Persistence.EF
{
    public record EventDto
    {
        public long Id { get; }
        public string EventType { get; }
        public string Data { get; }
        public DateTime CreatedAt { get; }

        public EventDto(Event ev)
        {
            EventType = ev.GetType().ToString();
            Data = JsonSerializer.Serialize(ev, ev.GetType());
            CreatedAt = ev.CreatedAt;
        }

        public TEvent ToEvent<TEvent>() where TEvent : Event
        {
            return JsonSerializer.Deserialize<TEvent>(Data);
        }

        private EventDto() { }
    }
}
