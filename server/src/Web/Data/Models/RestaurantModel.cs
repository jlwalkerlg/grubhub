using System;
using Web.Features.Menus;
using Web.Features.Restaurants;

namespace Web.Data.Models
{
    public record RestaurantModel
    {
        public Guid id { get; init; }
        public Guid manager_id { get; init; }
        public string name { get; init; }
        public string description { get; init; }
        public string phone_number { get; init; }
        public string address { get; init; }
        public float latitude { get; init; }
        public float longitude { get; init; }
        public string status { get; init; }
        public TimeSpan? monday_open { get; init; }
        public TimeSpan? monday_close { get; init; }
        public TimeSpan? tuesday_open { get; init; }
        public TimeSpan? tuesday_close { get; init; }
        public TimeSpan? wednesday_open { get; init; }
        public TimeSpan? wednesday_close { get; init; }
        public TimeSpan? thursday_open { get; init; }
        public TimeSpan? thursday_close { get; init; }
        public TimeSpan? friday_open { get; init; }
        public TimeSpan? friday_close { get; init; }
        public TimeSpan? saturday_open { get; init; }
        public TimeSpan? saturday_close { get; init; }
        public TimeSpan? sunday_open { get; init; }
        public TimeSpan? sunday_close { get; init; }
        public decimal delivery_fee { get; init; }
        public decimal minimum_delivery_spend { get; init; }
        public int max_delivery_distance_in_km { get; init; }
        public int estimated_delivery_time_in_minutes { get; init; }
        public MenuDto Menu { get; set; }

        public RestaurantDto ToDto()
        {
            return new()
            {
                Id = id,
                ManagerId = manager_id,
                Name = name,
                Description = description,
                PhoneNumber = phone_number,
                Address = address,
                Latitude = latitude,
                Longitude = longitude,
                Status = status,
                OpeningTimes = new OpeningTimesDto()
                {
                    Monday = monday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(monday_open),
                            Close = FormatTimeSpan(monday_close),
                        }
                        : null,
                    Tuesday = tuesday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(tuesday_open),
                            Close = FormatTimeSpan(tuesday_close),
                        }
                        : null,
                    Wednesday = wednesday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(wednesday_open),
                            Close = FormatTimeSpan(wednesday_close),
                        }
                        : null,
                    Thursday = thursday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(thursday_open),
                            Close = FormatTimeSpan(thursday_close),
                        }
                        : null,
                    Friday = friday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(friday_open),
                            Close = FormatTimeSpan(friday_close),
                        }
                        : null,
                    Saturday = saturday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(saturday_open),
                            Close = FormatTimeSpan(saturday_close),
                        }
                        : null,
                    Sunday = sunday_open.HasValue
                        ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(sunday_open),
                            Close = FormatTimeSpan(sunday_close),
                        }
                        : null,
                },
                DeliveryFee = delivery_fee,
                MinimumDeliverySpend = minimum_delivery_spend,
                MaxDeliveryDistanceInKm = max_delivery_distance_in_km,
                EstimatedDeliveryTimeInMinutes = estimated_delivery_time_in_minutes,
                Menu = Menu,
            };
        }

        private string FormatTimeSpan(TimeSpan? span)
        {
            return !span.HasValue
                ? null
                : $"{span?.Hours.ToString().PadLeft(2, '0')}:{span?.Minutes.ToString().PadLeft(2, '0')}";
        }
    }
}
