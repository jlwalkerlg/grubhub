using System;

namespace Web.Services.DateTimeServices
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
