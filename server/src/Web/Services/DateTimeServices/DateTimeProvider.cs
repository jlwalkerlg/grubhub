using System;

namespace Web.Services.DateTimeServices
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZoneInfo timeZone;

        public DateTimeProvider()
        {
            timeZone = Environment.OSVersion.Platform switch
            {
                PlatformID.Unix => TimeZoneInfo.FindSystemTimeZoneById("Europe/London"),
                PlatformID.Win32NT => TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"),
                _ => throw new Exception("Unknown OS platform."),
            };
        }

        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime GmtNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
}
