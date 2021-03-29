using System;

namespace Web.Domain.Orders
{
    public record OrderId
    {
        public OrderId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public string Value { get; }

        public static implicit operator string(OrderId id) => id.Value;
    }
}
