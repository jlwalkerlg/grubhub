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

        public bool IsOpen(DateTime time)
        {
            var hours = time.DayOfWeek switch
            {
                DayOfWeek.Monday => Monday,
                DayOfWeek.Tuesday => Tuesday,
                DayOfWeek.Wednesday => Wednesday,
                DayOfWeek.Thursday => Thursday,
                DayOfWeek.Friday => Friday,
                DayOfWeek.Saturday => Saturday,
                DayOfWeek.Sunday => Sunday,
                _ => throw new Exception("Day of week not recognised."),
            };

            if (hours == null)
            {
                return false;
            }

            var ts = new TimeSpan(time.Hour, time.Minute, time.Second);

            return hours.Open <= ts && (hours.Close == null || hours.Close >= ts);
        }

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

        public static OpeningTimes Always { get; } = new OpeningTimes()
        {
            Monday = new OpeningHours(TimeSpan.Zero),
            Tuesday = new OpeningHours(TimeSpan.Zero),
            Wednesday = new OpeningHours(TimeSpan.Zero),
            Thursday = new OpeningHours(TimeSpan.Zero),
            Friday = new OpeningHours(TimeSpan.Zero),
            Saturday = new OpeningHours(TimeSpan.Zero),
            Sunday = new OpeningHours(TimeSpan.Zero),
        };
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
