using System;

namespace Application.Events
{
    public abstract record Event
    {
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
