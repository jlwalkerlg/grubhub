namespace Web.Domain.Users
{
    public class Customer : User
    {
        public Customer(UserId id, string name, Email email, string password)
            : base(id, name, email, password)
        {
        }

        public override UserRole Role => UserRole.Customer;

        public MobileNumber MobileNumber { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}
