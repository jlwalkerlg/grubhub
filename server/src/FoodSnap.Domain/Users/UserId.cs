using System;

namespace FoodSnap.Domain.Users
{
    public record UserId(Guid Value) : GuidId(Value);
}
