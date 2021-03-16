namespace Web.Features.Restaurants.SearchRestaurants
{
    public record SearchRestaurantsQuery : IRequest<SearchRestaurantsResponse>
    {
        public string Postcode { get; init; }
        public RestaurantSearchOptions Options { get; init; }
    }
}
