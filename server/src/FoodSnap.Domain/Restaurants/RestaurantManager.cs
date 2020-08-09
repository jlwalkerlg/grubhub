using System;

namespace FoodSnap.Domain.Restaurants
{
    public class RestaurantManager : Entity
    {
        public string Name { get; }
        public Email Email { get; }
        public string Password { get; }
        public Guid RestaurantId { get; }

        public RestaurantManager(string name, Email email, string password, Guid restaurantId)
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

            if (restaurantId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(restaurantId)} must not be empty.");
            }

            Name = name;
            Email = email;
            Password = password;
            RestaurantId = restaurantId;
        }
    }
}
