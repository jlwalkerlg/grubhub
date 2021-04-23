using System;
using TimeZoneConverter;

namespace Web.Services.DateTimeServices
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        public TimeZoneInfo BritishTimeZone => TZConvert.GetTimeZoneInfo("Europe/London");
    }
}
