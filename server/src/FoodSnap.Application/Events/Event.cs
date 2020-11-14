using System;

namespace FoodSnap.Application.Events
{
    public abstract record Event
    {
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
