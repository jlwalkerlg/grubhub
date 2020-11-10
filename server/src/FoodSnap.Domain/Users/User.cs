using System;

namespace FoodSnap.Domain.Users
{
    public abstract class User : Entity<User>
    {
        public User(UserId id, string name, Email email, string password)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password must not be empty.");
            }

            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public UserId Id { get; }

        private string name;
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

        private Email email;
        public Email Email
        {
            get => email;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(Email));
                }

                email = value;
            }
        }

        public string Password { get; }

        public abstract UserRole Role { get; }

        protected override bool IdentityEquals(User other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // EF Core
        protected User() { }
    }
}
