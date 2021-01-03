using Domain.Restaurants;

namespace Infrastructure.Persistence.EF
{
    public class RestaurantCuisine
    {
        public RestaurantId RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public string CuisineName { get; set; }
        public Cuisine Cuisine { get; set; }
    }
}
