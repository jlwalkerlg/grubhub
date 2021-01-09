using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("restaurants")]
    public record Restaurant
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("manager_id")]
        public Guid ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public User Manager { get; set; }

        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = "01234567890";

        [Column("address")]
        public string Address { get; set; } = "12 Maine Road, Manchester, UK";

        [Column("latitude")]
        public float Latitude { get; set; } = 54.0f;

        [Column("longitude")]
        public float Longitude { get; set; } = -2.0f;

        [Column("status")]
        public string Status { get; set; } = "Approved";

        [Column("monday_open")]
        public TimeSpan? MondayOpen { get; set; }

        [Column("monday_close")]
        public TimeSpan? MondayClose { get; set; }

        [Column("tuesday_open")]
        public TimeSpan? TuesdayOpen { get; set; }

        [Column("tuesday_close")]
        public TimeSpan? TuesdayClose { get; set; }

        [Column("wednesday_open")]
        public TimeSpan? WednesdayOpen { get; set; }

        [Column("wednesday_close")]
        public TimeSpan? WednesdayClose { get; set; }

        [Column("thursday_open")]
        public TimeSpan? ThursdayOpen { get; set; }

        [Column("thursday_close")]
        public TimeSpan? ThursdayClose { get; set; }

        [Column("friday_open")]
        public TimeSpan? FridayOpen { get; set; }

        [Column("friday_close")]
        public TimeSpan? FridayClose { get; set; }

        [Column("saturday_open")]
        public TimeSpan? SaturdayOpen { get; set; }

        [Column("saturday_close")]
        public TimeSpan? SaturdayClose { get; set; }

        [Column("sunday_open")]
        public TimeSpan? SundayOpen { get; set; }

        [Column("sunday_close")]
        public TimeSpan? SundayClose { get; set; }

        [Column("minimum_delivery_spend")]
        public decimal MinimumDeliverySpend { get; set; } = 10m;

        [Column("delivery_fee")]
        public decimal DeliveryFee { get; set; } = 1.5m;

        [Column("max_delivery_distance_in_km")]
        public int MaxDeliveryDistanceInKm { get; set; } = 5;

        [Column("estimated_delivery_time_in_minutes")]
        public int EstimatedDeliveryTimeInMinutes { get; set; } = 40;

        public List<Cuisine> Cuisines { get; set; } = new();
    }
}
