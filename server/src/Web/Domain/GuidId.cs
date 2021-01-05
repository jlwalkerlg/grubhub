using System;

namespace Web.Domain
{
    public record GuidId
    {
        public GuidId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Id must not be empty.");
            }

            Value = value;
        }

        public Guid Value { get; }
    }
}
