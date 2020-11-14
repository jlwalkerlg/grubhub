namespace FoodSnap.Web.Actions.Restaurants.UpdateRestaurantDetails
{
    public record UpdateRestaurantDetailsRequest
    {
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
    }
}
