using System;

namespace Web.Domain.Billing
{
    public class BillingAccount : Entity<BillingAccount>
    {
        public BillingAccount(BillingAccountId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public BillingAccountId Id { get; }
        public bool Enabled { get; private set; } = false;

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected override bool IdentityEquals(BillingAccount other)
        {
            return Id == other.Id;
        }
    }
}
