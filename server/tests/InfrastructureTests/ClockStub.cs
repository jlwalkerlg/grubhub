using System;
using Application.Services;

namespace InfrastructureTests
{
    public class ClockStub : IClock
    {
        public DateTime UtcNow { get; set; }
    }
}
