namespace FoodSnap.Domain.Users
{
    public class Admin : User
    {
        public Admin(UserId id, string name, Email email, string password)
            : base(id, name, email, password)
        {
        }

        public override UserRole Role => UserRole.Admin;
    }
}
