using System;

namespace Web.Services.DateTimeServices
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
