using System;

namespace FoodSnap.Domain.Users
{
    public abstract class User : Entity
    {
        public string Name { get; }
        public Email Email { get; }
        public string Password { get; }

        protected abstract UserRole Role { get; }

        public User(string name, Email email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} must not be empty.");
            }

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

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
