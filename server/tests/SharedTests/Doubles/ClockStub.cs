using System;
using Application.Services;

namespace SharedTests.Doubles
{
    public class ClockStub : IClock
    {
        public DateTime UtcNow { get; set; }
    }
}
