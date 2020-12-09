using System;

namespace FoodSnap.Application.Services
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
