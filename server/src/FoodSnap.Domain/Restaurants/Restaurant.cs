using System;

namespace FoodSnap.Domain.Restaurants
{
    public class Restaurant : Entity
    {
        public Guid ManagerId { get; }
        public string Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Address Address { get; }
        public Coordinates Coordinates { get; }
        public RestaurantApplicationStatus Status { get; private set; }

        public Restaurant(
            Guid managerId,
            string name,
            PhoneNumber phoneNumber,
            Address address,
            Coordinates coordinates)
        {
            if (managerId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(managerId)} must not be empty.");
            }

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

            ManagerId = managerId;
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Coordinates = coordinates;
            Status = RestaurantApplicationStatus.Pending;
        }

        public void AcceptApplication()
        {
            Status = RestaurantApplicationStatus.Accepted;
        }

        // EF Core
        private Restaurant() { }
    }
}
