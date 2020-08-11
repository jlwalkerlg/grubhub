using System;

namespace FoodSnap.Domain.Restaurants
{
    public class Restaurant : Entity
    {
        public string Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Address Address { get; }

        public Restaurant(string name, PhoneNumber phoneNumber, Address address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} must not be empty.");
            }

            if (phoneNumber is null)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            if (address is null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        // EF Core
        private Restaurant() { }
    }
}
