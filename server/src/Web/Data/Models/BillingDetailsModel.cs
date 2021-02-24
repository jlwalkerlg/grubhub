using System;
using Web.Features.Billing.GetBillingDetails;

namespace Web.Data.Models
{
    public record BillingDetailsModel
    {
        public string id { get; init; }
        public Guid restaurant_id { get; init; }
        public bool billing_enabled { get; init; }
        public Guid manager_id { get; init; }

        public BillingDetails ToDto()
        {
            return new BillingDetails()
            {
                Id = id,
                RestaurantId = restaurant_id,
                IsBillingEnabled = billing_enabled,
            };
        }
    }
}
