using FoodSnap.Domain;
using FoodSnap.Domain.Users;

namespace FoodSnap.DomainTests.Users
{
    public class DummyUser : User
    {
        protected override UserRole Role => UserRole.Customer;

        public DummyUser(string name, Email email, string password) : base(name, email, password)
        {
        }
    }
}
