using FoodSnap.Web.Queries.Restaurants;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Queries.Auth
{
    public class AuthDataDto
    {
        public UserDto User { get; set; }
        public RestaurantDto Restaurant { get; set; }
    }
}
