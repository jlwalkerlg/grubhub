using System;

namespace Web.Domain.Users
{
    public record UserId(Guid Value) : GuidId(Value);
}
