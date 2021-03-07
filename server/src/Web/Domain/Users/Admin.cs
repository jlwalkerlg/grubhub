namespace Web.Domain.Users
{
    public class Admin : User
    {
        public Admin(UserId id, string firstName, string lastName, Email email, string password)
            : base(id, firstName, lastName, email, password)
        {
        }

        public override UserRole Role => UserRole.Admin;
    }
}
