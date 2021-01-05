using System;
using System.Collections.Generic;

namespace Web.Domain.Restaurants
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

        public static OpeningTimes FromDays(Dictionary<DayOfWeek, OpeningHours> times)
        {
            return new OpeningTimes()
            {
                Monday = times.GetValueOrDefault(DayOfWeek.Monday),
                Tuesday = times.GetValueOrDefault(DayOfWeek.Tuesday),
                Wednesday = times.GetValueOrDefault(DayOfWeek.Wednesday),
                Thursday = times.GetValueOrDefault(DayOfWeek.Thursday),
                Friday = times.GetValueOrDefault(DayOfWeek.Friday),
                Saturday = times.GetValueOrDefault(DayOfWeek.Saturday),
                Sunday = times.GetValueOrDefault(DayOfWeek.Sunday),
            };
        }
    }

    public record OpeningHours
    {
        public OpeningHours(TimeSpan open, TimeSpan? close = null)
        {
            if (close.HasValue && close.Value <= open)
            {
                throw new ArgumentException(
                    "Closing time must be greater than opening time.",
                    nameof(close));
            }

            Open = open;
            Close = close;
        }

        public TimeSpan Open { get; }
        public TimeSpan? Close { get; }

        private OpeningHours() { }

        public static OpeningHours Parse(string open, string close)
        {
            if (string.IsNullOrWhiteSpace(open))
            {
                return null;
            }

            return new OpeningHours(
                TimeSpan.Parse(open),
                string.IsNullOrWhiteSpace(close) ? null : TimeSpan.Parse(close));
        }
    }
}
