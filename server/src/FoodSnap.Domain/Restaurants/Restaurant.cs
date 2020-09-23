using System;

namespace FoodSnap.Domain.Restaurants
{
    public class Restaurant : Entity
    {
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
            Status = RestaurantStatus.PendingApproval;
        }

        public Guid ManagerId { get; }

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

        private PhoneNumber phoneNumber;
        public PhoneNumber PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(PhoneNumber));
                }

                phoneNumber = value;
            }
        }

        public Address Address { get; }

        public Coordinates Coordinates { get; }

        public RestaurantStatus Status { get; private set; }

        public void Approve()
        {
            if (Status != RestaurantStatus.PendingApproval)
            {
                throw new InvalidOperationException("Restaurant already approved.");
            }

            Status = RestaurantStatus.Approved;
        }

        // EF Core
        private Restaurant() { }
    }
}
