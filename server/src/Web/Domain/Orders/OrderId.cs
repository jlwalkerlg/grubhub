using System;

namespace Web.Domain.Orders
{
    public record OrderId(Guid Value) : GuidId(Value);
}
