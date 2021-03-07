using System;

namespace Web.Domain.Users
{
    public abstract class User : Entity<User>
    {
        private string firstName;
        private string lastName;
        private Email email;

        protected User() { } // EF Core

        protected User(UserId id, string firstName, string lastName, Email email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password must not be empty.");
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public UserId Id { get; }

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("First name must not be empty.");
                }

                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Last name must not be empty.");
                }

                lastName = value;
            }
        }

        public string Name => $"{FirstName} {LastName}";

        public Email Email
        {
            get => email;
            set => email = value ?? throw new ArgumentNullException(nameof(Email));
        }

        public string Password { get; set; }
        public MobileNumber MobileNumber { get; set; }
        public Address DeliveryAddress { get; set; }

        public abstract UserRole Role { get; }

        public void Rename(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

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
