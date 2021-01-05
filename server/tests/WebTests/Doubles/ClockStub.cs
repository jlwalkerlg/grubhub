using System;
using Web.Services;

namespace WebTests.Doubles
{
    public class ClockStub : IClock
    {
        public DateTime UtcNow { get; set; }
    }
}
