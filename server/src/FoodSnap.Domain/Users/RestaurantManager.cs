namespace FoodSnap.Domain.Users
{
    public class RestaurantManager : User
    {
        protected override UserRole Role => UserRole.RestaurantManager;

        public RestaurantManager(string name, Email email, string password)
            : base(name, email, password)
        {
        }

        // EF Core
        private RestaurantManager() { }
    }
}
