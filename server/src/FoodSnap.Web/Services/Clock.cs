using System;
using FoodSnap.Application.Services;

namespace FoodSnap.Web.Services
{
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
