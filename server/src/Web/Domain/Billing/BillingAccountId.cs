using System;

namespace Web.Domain.Billing
{
    public record BillingAccountId
    {
        public BillingAccountId(string id)
        {
            Value = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(BillingAccountId id) => id.Value;
    }
}
