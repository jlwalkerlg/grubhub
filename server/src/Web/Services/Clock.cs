using System;

namespace Web.Services
{
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
