namespace FoodSnap.Domain.Users
{
    public class Admin : User
    {
        protected override UserRole Role => UserRole.Admin;

        public Admin(string name, Email email, string password) : base(name, email, password)
        {
        }
    }
}
