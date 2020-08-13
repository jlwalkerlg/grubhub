using System;

namespace FoodSnap.Domain.Restaurants
{
    public class Restaurant : Entity
    {
        public string Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Address Address { get; }
        public Coordinates Coordinates { get; }

        public Restaurant(string name, PhoneNumber phoneNumber, Address address, Coordinates coordinates)
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

            if (coordinates is null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Coordinates = coordinates;
        }

        // EF Core
        private Restaurant() { }
    }
}
