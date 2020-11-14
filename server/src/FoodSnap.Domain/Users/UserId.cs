using System;

namespace FoodSnap.Domain.Users
{
    public record UserId(Guid value) : GuidId(value);
}
