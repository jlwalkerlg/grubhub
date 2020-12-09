using System;
using FoodSnap.Application.Services;

namespace FoodSnap.WebTests.Doubles
{
    public class ClockStub : IClock
    {
        public static DateTime Now { get; } = DateTime.UtcNow;
        public DateTime UtcNow => Now;
    }
}
