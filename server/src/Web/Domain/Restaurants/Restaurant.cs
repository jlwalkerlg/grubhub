using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Baskets;
using Web.Domain.Billing;
using Web.Domain.Cuisines;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Users;

namespace Web.Domain.Restaurants
{
    public class Restaurant : Entity<Restaurant>
    {
        private string name;
        private string description;
        private PhoneNumber phoneNumber;
        private Money minimumDeliverySpend = Money.Zero;
        private Money deliveryFee = Money.Zero;
        public Distance maxDeliveryDistance = Distance.Zero;
        private int estimatedDeliveryTime = 30;
        private readonly List<Cuisine> _cuisines = new();

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

        private Restaurant() { } // EF

        public RestaurantId Id { get; }

        public UserId ManagerId { get; }

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

        public string Description
        {
            get => description;
            set
            {
                if (value?.Length > 400)
                {
                    throw new ArgumentException("Description must not be longer than 400 characters.");
                }

                description = value;
            }
        }

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

        public Distance MaxDeliveryDistance
        {
            get => maxDeliveryDistance;
            set
            {
                if (value.Km < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxDeliveryDistance));
                }

                maxDeliveryDistance = value;
            }
        }

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

        public Result<Order> PlaceOrder(
            OrderId orderId,
            Basket basket,
            Menu menu,
            MobileNumber mobileNumber,
            DeliveryLocation deliveryLocation,
            BillingAccount billingAccount,
            DateTime time)
        {
            if (basket.RestaurantId != Id)
            {
                throw new InvalidOperationException("Basket belongs to another restaurant.");
            }

            if (!billingAccount.Enabled)
            {
                return Error.BadRequest("Restaurant not accepting orders.");
            }

            var timeAtDelivery = time.AddMinutes(EstimatedDeliveryTimeInMinutes);

            if (OpeningTimes == null || !OpeningTimes.IsOpen(timeAtDelivery))
            {
                return Error.BadRequest("Restaurant is closed at time of delivery.");
            }

            var distance = deliveryLocation.Coordinates.CalculateDistance(Coordinates);

            if (distance > maxDeliveryDistance)
            {
                return Error.BadRequest($"Delivery beyond {maxDeliveryDistance.Km} km not available.");
            }

            var subtotal = basket.CalculateSubtotal(menu);

            if (subtotal < MinimumDeliverySpend)
            {
                return Error.BadRequest("Order subtotal not enough for delivery.");
            }

            return Result.Ok(
                new Order(
                    orderId,
                    basket,
                    subtotal,
                    DeliveryFee,
                    mobileNumber,
                    deliveryLocation.Address,
                    time));
        }

        protected override bool IdentityEquals(Restaurant other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
