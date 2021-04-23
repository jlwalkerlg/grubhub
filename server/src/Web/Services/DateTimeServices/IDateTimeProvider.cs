using System;

namespace Web.Services.DateTimeServices
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}
