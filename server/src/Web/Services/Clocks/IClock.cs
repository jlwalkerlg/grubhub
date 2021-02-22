using System;

namespace Web.Services.Clocks
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
