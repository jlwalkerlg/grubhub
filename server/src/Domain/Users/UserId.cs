using System;

namespace Domain.Users
{
    public record UserId(Guid Value) : GuidId(Value);
}
