using System;
using System.Text.Json;
using Web.Features.Events;

namespace Web.Data.EF
{
    public record EventDto
    {
        public long Id { get; }
        public string EventType { get; }
        public string Data { get; }
        public DateTime CreatedAt { get; }
        public bool Handled { get; private set; } = false;

        public EventDto(Event ev)
        {
            EventType = ev.GetType().ToString();
            Data = JsonSerializer.Serialize(ev, ev.GetType());
            CreatedAt = ev.CreatedAt;
        }

        private EventDto() { } // EF

        public TEvent ToEvent<TEvent>() where TEvent : Event
        {
            return JsonSerializer.Deserialize<TEvent>(Data);
        }

        public void MarkHandled()
        {
            Handled = true;
        }
    }
}
