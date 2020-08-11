using System;

namespace FoodSnap.Application
{
    public abstract class Event
    {
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }
}
