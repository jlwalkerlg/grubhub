using System;
using TimeZoneConverter;
using Web.Services.DateTimeServices;

namespace WebTests.Doubles
{
    public class DateTimeProviderStub : IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; set; }
        public TimeZoneInfo BritishTimeZone => TZConvert.GetTimeZoneInfo("Europe/London");
    }
}
