namespace FoodSnap.Web.Actions.Restaurants.UpdateRestaurantDetails
{
    public record UpdateRestaurantDetailsRequest
    {
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
        public decimal DeliveryFee { get; init; }
        public decimal MinimumDeliverySpend { get; init; }
        public int MaxDeliveryDistanceInKm { get; init; }
        public int EstimatedDeliveryTimeInMinutes { get; init; }
    }
}
