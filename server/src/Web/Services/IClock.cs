using System;

namespace Web.Services
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
