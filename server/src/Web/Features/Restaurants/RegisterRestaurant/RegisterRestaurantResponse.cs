namespace Web.Features.Restaurants.RegisterRestaurant
{
    public record RegisterRestaurantResponse
    {
        public string XsrfToken { get; init; }
    }
}