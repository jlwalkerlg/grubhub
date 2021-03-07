namespace Web.Domain.Users
{
    public class RestaurantManager : User
    {
        public RestaurantManager(UserId id, string firstName, string lastName, Email email, string password)
            : base(id, firstName, lastName, email, password)
        {
        }

        public override UserRole Role => UserRole.RestaurantManager;

        // EF Core
        private RestaurantManager() { }
    }
}
