namespace Domain.Users
{
    public class RestaurantManager : User
    {
        public RestaurantManager(UserId id, string name, Email email, string password)
            : base(id, name, email, password)
        {
        }

        public override UserRole Role => UserRole.RestaurantManager;

        // EF Core
        private RestaurantManager() { }
    }
}
