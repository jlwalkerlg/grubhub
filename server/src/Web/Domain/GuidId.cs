using System;

namespace Web.Domain
{
    public abstract record GuidId
    {
        protected GuidId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Id must not be empty.");
            }

            Value = value;
        }

        public Guid Value { get; }

        public static implicit operator Guid(GuidId id) => id.Value;
    }
}
