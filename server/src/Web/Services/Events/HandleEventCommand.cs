using System;

namespace Web.Services.Events
{
    public record HandleEventCommand<TEvent> : IRequest where TEvent : Event
    {
        public HandleEventCommand(TEvent ev)
        {
            Event = ev ?? throw new ArgumentNullException(nameof(ev));
        }

        public TEvent Event { get; }
    }
}