using System;
using System.Collections.Generic;
using Shouldly;
using Web.Domain.Restaurants;
using Xunit;

namespace WebTests.Domain.Restaurants
{
    public class OpeningTimesTests
    {
        [Fact]
        public void IsOpen()
        {
            OpeningTimes times;

            times = new OpeningTimes();

            times.IsOpen(DateTimeOffset.UtcNow).ShouldBe(false);

            times = OpeningTimes.Always;

            times.IsOpen(DateTimeOffset.UtcNow).ShouldBe(true);

            times = new OpeningTimes()
            {
                Monday = OpeningHours.Parse("01:00", "02:00"),
                Tuesday = OpeningHours.Parse("00:00", null),
            };

            times.IsOpen(DateTimeOffset.Parse("Mon 01 Feb 2021 00:00:00 GMT")).ShouldBe(false);
            times.IsOpen(DateTimeOffset.Parse("Mon 01 Feb 2021 01:00:00 GMT")).ShouldBe(true);
            times.IsOpen(DateTimeOffset.Parse("Mon 01 Feb 2021 02:00:00 GMT")).ShouldBe(true);
            times.IsOpen(DateTimeOffset.Parse("Mon 01 Feb 2021 02:00:01 GMT")).ShouldBe(false);
            times.IsOpen(DateTimeOffset.Parse("Tue 02 Feb 2021 00:00:00 GMT")).ShouldBe(true);
        }

        [Fact]
        public void Parse()
        {
            var times = OpeningTimes.FromDays(new Dictionary<DayOfWeek, OpeningHours>()
            {
                { DayOfWeek.Monday, OpeningHours.Parse("16:00", "23:00") },
            });

            times.Monday.Open.ShouldBe(TimeSpan.Parse("16:00"));
            times.Monday.Close.ShouldBe(TimeSpan.Parse("23:00"));

            times.Tuesday.ShouldBe(null);

            times.IsOpen(DateTimeOffset.Parse("Mon 01 Feb 2021 16:00:00 GMT")).ShouldBe(true);
        }
    }
}
