using System;
using Application.Services;

namespace WebTests.Doubles
{
    public class ClockStub : IClock
    {
        public static DateTime Now { get; } = DateTime.UtcNow;
        public DateTime UtcNow => Now;
    }
}
