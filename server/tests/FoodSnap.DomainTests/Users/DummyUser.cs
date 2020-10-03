using FoodSnap.Domain;
using FoodSnap.Domain.Users;

namespace FoodSnap.DomainTests.Users
{
    public class DummyUser : User
    {
        public override UserRole Role => UserRole.Customer;

        public DummyUser(UserId id, string name, Email email, string password)
            : base(id, name, email, password)
        {
        }
    }
}
