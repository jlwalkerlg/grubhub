using System.Collections.Generic;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public record RestaurantSearchOptions
    {
        public string SortBy { get; set; }
        public List<string> Cuisines { get; set; } = new();
        public int? Page { get; init; }
        public int? PerPage { get; init; }
    }
}
