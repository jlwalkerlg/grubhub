using System;
using TimeZoneConverter;
using Web.Services.DateTimeServices;

namespace WebTests.Doubles
{
    public class DateTimeProviderStub : IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
        public TimeZoneInfo BritishTimeZone { get; } = TZConvert.GetTimeZoneInfo("Europe/London");
    }
}
