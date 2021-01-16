using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Cuisines;
using Web.Domain.Users;

namespace Web.Domain.Restaurants
{
    public class Restaurant : Entity<Restaurant>
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
                    throw new ArgumentException("Name must not be empty.");
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

        public OpeningTimes OpeningTimes { get; set; }

        private Money minimumDeliverySpend = new(0);
        public Money MinimumDeliverySpend
        {
            get => minimumDeliverySpend;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(MinimumDeliverySpend));
                }

                minimumDeliverySpend = value;
            }
        }

        private Money deliveryFee = new(0);
        public Money DeliveryFee
        {
            get => deliveryFee;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(DeliveryFee));
                }

                deliveryFee = value;
            }
        }

        public int maxDeliveryDistanceInKm;
        public int MaxDeliveryDistanceInKm
        {
            get => maxDeliveryDistanceInKm;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxDeliveryDistanceInKm));
                }

                maxDeliveryDistanceInKm = value;
            }
        }

        private int estimatedDeliveryTime = 30;
        public int EstimatedDeliveryTimeInMinutes
        {
            get => estimatedDeliveryTime;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Delivery time must be greater than zero.");
                }

                estimatedDeliveryTime = value;
            }
        }

        private readonly List<Cuisine> _cuisines = new();
        public IReadOnlyList<Cuisine> Cuisines => _cuisines;

        public void SetCuisines(params Cuisine[] cuisines)
        {
            _cuisines.RemoveAll(x => !cuisines.Contains(x));
            _cuisines.AddRange(cuisines.Where(x => !_cuisines.Contains(x)));
        }

        public void SetCuisines(IEnumerable<Cuisine> cuisines)
        {
            SetCuisines(cuisines.ToArray());
        }

        public void Approve()
        {
            if (Status != RestaurantStatus.PendingApproval)
            {
                throw new InvalidOperationException("Restaurant already approved.");
            }

            Status = RestaurantStatus.Approved;
        }

        protected override bool IdentityEquals(Restaurant other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // EF Core
        private Restaurant() { }
    }
}