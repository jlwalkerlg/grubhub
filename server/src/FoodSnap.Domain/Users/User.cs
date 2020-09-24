using System;

namespace FoodSnap.Domain.Users
{
    public abstract class User : Entity
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{nameof(Name)} must not be empty.");
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

        public User(string name, Email email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"{nameof(password)} must not be empty.");
            }

            Name = name;
            Email = email;
            Password = password;
        }

        // EF Core
        protected User() { }
    }
}
