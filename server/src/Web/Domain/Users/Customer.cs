namespace Web.Domain.Users
{
    public class Customer : User
    {
        public Customer(UserId id, string firstName, string lastName, Email email, string password)
            : base(id, firstName, lastName, email, password)
        {
        }

        public override UserRole Role => UserRole.Customer;
    }
}
