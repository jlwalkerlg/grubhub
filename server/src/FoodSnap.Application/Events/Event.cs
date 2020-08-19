using System;

namespace FoodSnap.Application.Events
{
    public abstract class Event
    {
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }
}
