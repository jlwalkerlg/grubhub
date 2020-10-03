using System;
using FoodSnap.Domain.Users;

namespace FoodSnap.Domain.Restaurants
{
    public class Restaurant : Entity<RestaurantId>
    {
        public Restaurant(
            RestaurantId id,
            UserId managerId,
            string name,
            PhoneNumber phoneNumber,
            Address address,
            Coordinates coordinates)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (managerId == null)
            {
                throw new ArgumentNullException(nameof(managerId));
            }

            if (address is null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (coordinates is null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            Id = id;
            ManagerId = managerId;
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Coordinates = coordinates;
            Status = RestaurantStatus.PendingApproval;
        }

        protected override RestaurantId ID => Id;
        public RestaurantId Id { get; }

        public UserId ManagerId { get; }

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
