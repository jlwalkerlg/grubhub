using System;
using Web.Services.Clocks;

namespace WebTests.Doubles
{
    public class ClockStub : IClock
    {
        public DateTime UtcNow { get; set; }
    }
}
