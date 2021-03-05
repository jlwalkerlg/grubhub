using System;

namespace Web.Features.Restaurants.UpdateRestaurantDetails
{
    public record UpdateRestaurantDetailsCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string PhoneNumber { get; init; }
        public decimal DeliveryFee { get; init; }
        public decimal MinimumDeliverySpend { get; init; }
        public int MaxDeliveryDistanceInKm { get; init; }
        public int EstimatedDeliveryTimeInMinutes { get; init; }
    }
}
