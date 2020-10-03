using System;

namespace FoodSnap.Domain
{
    public class GuidId : ValueObject<GuidId>
    {
        public GuidId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(value)} is be empty.");
            }

            Value = value;
        }

        public Guid Value { get; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool IsEqual(GuidId other)
        {
            return Value == other.Value;
        }
    }
}
