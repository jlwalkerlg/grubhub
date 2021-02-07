using System;
using Web.Domain.Restaurants;

namespace Web.Domain.Billing
{
    public class BillingAccount : Entity<BillingAccount>
    {
        public BillingAccount(BillingAccountId id, RestaurantId restaurantId)
        {
            Id = id ??
                throw new ArgumentNullException(nameof(id));

            RestaurantId = restaurantId ??
                throw new ArgumentNullException(nameof(restaurantId));
        }

        public BillingAccountId Id { get; }
        public RestaurantId RestaurantId { get; }
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
