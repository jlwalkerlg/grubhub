using System;

namespace Web.Domain.Users
{
    public abstract class User : Entity<User>
    {
        private string name;
        private Email email;

        protected User() { } // EF Core

        protected User(UserId id, string name, Email email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password must not be empty.");
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            Email = email;
            Password = password;
        }

        public UserId Id { get; }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name must not be empty.");
                }

                name = value;
            }
        }

        public Email Email
        {
            get => email;
            set => email = value ?? throw new ArgumentNullException(nameof(Email));
        }

        public string Password { get; set; }
        public MobileNumber MobileNumber { get; set; }
        public Address DeliveryAddress { get; set; }

        public abstract UserRole Role { get; }

        protected override bool IdentityEquals(User other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
