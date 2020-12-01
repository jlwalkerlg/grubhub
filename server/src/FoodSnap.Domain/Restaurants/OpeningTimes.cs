using System;

namespace FoodSnap.Domain.Restaurants
{
    public record OpeningTimes
    {
        public OpeningHours Monday { get; init; }
        public OpeningHours Tuesday { get; init; }
        public OpeningHours Wednesday { get; init; }
        public OpeningHours Thursday { get; init; }
        public OpeningHours Friday { get; init; }
        public OpeningHours Saturday { get; init; }
        public OpeningHours Sunday { get; init; }
    }

    public record OpeningHours
    {
        public OpeningHours(TimeSpan open, TimeSpan close)
        {
            Open = open;
            Close = close;
        }

        public TimeSpan Open { get; }
        public TimeSpan Close { get; }

        private OpeningHours() { }
    }
}
