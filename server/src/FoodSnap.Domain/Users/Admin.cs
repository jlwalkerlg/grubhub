namespace FoodSnap.Domain.Users
{
    public class Admin : User
    {
        public override UserRole Role => UserRole.Admin;

        public Admin(string name, Email email, string password) : base(name, email, password)
        {
        }
    }
}
