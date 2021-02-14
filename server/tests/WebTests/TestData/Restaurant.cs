using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace WebTests.TestData
{
    [Table("restaurants")]
    public record Restaurant
    {
        private User manager;

        public Restaurant()
        {
            Manager = new User()
            {
                Role = UserRole.RestaurantManager,
            };

            Menu = new Menu()
            {
                RestaurantId = Id,
            };

            BillingAccount = new BillingAccount()
            {
                RestaurantId = Id,
            };
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("manager_id")]
        public Guid ManagerId { get; private set; }

        [ForeignKey(nameof(ManagerId))]
        public User Manager
        {
            get => manager;
            set
            {
                manager = value;
                ManagerId = value == null ? Guid.Empty : value.Id;
            }
        }

        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Column("description")]
        public string Description { get; set; } = Guid.NewGuid().ToString();

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = "01234567890";

        [Column("address")]
        public string Address { get; set; } = "12 Maine Road, Manchester, UK";

        [Column("latitude")]
        public float Latitude { get; set; } = 54.0f;

        [Column("longitude")]
        public float Longitude { get; set; } = -2.0f;

        [Column("status")]
        public RestaurantStatus Status { get; set; } = RestaurantStatus.Approved;

        [Column("monday_open")]
        public TimeSpan? MondayOpen { get; set; } = TimeSpan.Zero;

        [Column("monday_close")]
        public TimeSpan? MondayClose { get; set; }

        [Column("tuesday_open")]
        public TimeSpan? TuesdayOpen { get; set; } = TimeSpan.Zero;

        [Column("tuesday_close")]
        public TimeSpan? TuesdayClose { get; set; }

        [Column("wednesday_open")]
        public TimeSpan? WednesdayOpen { get; set; } = TimeSpan.Zero;

        [Column("wednesday_close")]
        public TimeSpan? WednesdayClose { get; set; }

        [Column("thursday_open")]
        public TimeSpan? ThursdayOpen { get; set; } = TimeSpan.Zero;

        [Column("thursday_close")]
        public TimeSpan? ThursdayClose { get; set; }

        [Column("friday_open")]
        public TimeSpan? FridayOpen { get; set; } = TimeSpan.Zero;

        [Column("friday_close")]
        public TimeSpan? FridayClose { get; set; }

        [Column("saturday_open")]
        public TimeSpan? SaturdayOpen { get; set; } = TimeSpan.Zero;

        [Column("saturday_close")]
        public TimeSpan? SaturdayClose { get; set; }

        [Column("sunday_open")]
        public TimeSpan? SundayOpen { get; set; } = TimeSpan.Zero;

        [Column("sunday_close")]
        public TimeSpan? SundayClose { get; set; }

        [Column("minimum_delivery_spend")]
        public int MinimumDeliverySpend { get; set; } = 1000;

        [Column("delivery_fee")]
        public int DeliveryFee { get; set; } = 1500;

        [Column("max_delivery_distance_in_km")]
        public int MaxDeliveryDistanceInKm { get; set; } = 5;

        [Column("estimated_delivery_time_in_minutes")]
        public int EstimatedDeliveryTimeInMinutes { get; set; } = 40;

        public List<Cuisine> Cuisines { get; set; } = new();

        public Menu Menu { get; set; }

        public BillingAccount BillingAccount { get; set; }
    }
}
