using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Features.Cuisines;
using Web.Features.Restaurants;
using Web.Services;

namespace Web.Data.Dapper.Repositories.Restaurants
{
    public class DPRestaurantDtoRepository : IRestaurantDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IClock clock;

        public DPRestaurantDtoRepository(
            IDbConnectionFactory dbConnectionFactory, IClock clock)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.clock = clock;
        }

        public async Task<List<RestaurantDto>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null)
        {
            var sql = @"
                SELECT
                    r.id,
                    r.manager_id,
                    r.name,
                    r.phone_number,
                    r.address,
                    r.latitude,
                    r.longitude,
                    r.status,
                    r.monday_open,
                    r.monday_close,
                    r.tuesday_open,
                    r.tuesday_close,
                    r.wednesday_open,
                    r.wednesday_close,
                    r.thursday_open,
                    r.thursday_close,
                    r.friday_open,
                    r.friday_close,
                    r.saturday_open,
                    r.saturday_close,
                    r.sunday_open,
                    r.sunday_close,
                    r.delivery_fee,
                    r.minimum_delivery_spend,
                    r.max_delivery_distance_in_km,
                    r.estimated_delivery_time_in_minutes
                FROM
                    restaurants r
                WHERE
                    r.status = @Status";

            var now = clock.UtcNow;
            var day = now.DayOfWeek.ToString().ToLower();

            sql += $" AND {day}_open <= @Now AND ({day}_close IS NULL OR {day}_close > @Now)";

            sql += " AND FLOOR(6371000 * acos(sin(radians(r.latitude)) * sin(radians(@OriginLatitude)) + cos(radians(r.latitude)) * cos(radians(@OriginLatitude)) * cos(radians(@OriginLongitude - r.longitude)))) / 1000 <= r.max_delivery_distance_in_km";

            if (options?.Cuisines.Count > 0)
            {
                sql += @" AND r.id = ANY(
                    SELECT DISTINCT
                        rc.restaurant_id
                    FROM
                        restaurant_cuisines rc
                        INNER JOIN cuisines c ON c.name = rc.cuisine_name
                    WHERE
                        c.name = ANY(@Cuisines))";
            }

            if (options?.SortBy == "distance")
            {
                sql += " ORDER BY (FLOOR(6371000 * acos(sin(radians(r.latitude)) * sin(radians(@OriginLatitude)) + cos(radians(r.latitude)) * cos(radians(@OriginLatitude)) * cos(radians(@OriginLongitude - r.longitude)))) / 1000) ASC";
            }
            else if (options?.SortBy == "min_order")
            {
                sql += " ORDER BY r.minimum_delivery_spend ASC";
            }
            else if (options?.SortBy == "delivery_fee")
            {
                sql += " ORDER BY r.delivery_fee ASC";
            }
            else if (options?.SortBy == "time")
            {
                sql += " ORDER BY r.estimated_delivery_time_in_minutes ASC";
            }

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var entries = await connection
                    .QueryAsync<RestaurantEntry>(
                        sql,
                        new
                        {
                            Status = RestaurantStatus.Approved.ToString(),
                            Now = now.TimeOfDay,
                            OriginLatitude = coordinates.Latitude,
                            OriginLongitude = coordinates.Longitude,
                            Cuisines = options?.Cuisines,
                        });

                var restaurants = entries.Select(EntryToDto).ToList();

                var cuisines = await connection
                    .QueryAsync<RestaurantCuisine>(
                        @"SELECT
                            rc.restaurant_id,
                            rc.cuisine_name
                        FROM
                            restaurant_cuisines rc
                        WHERE
                            rc.restaurant_id = ANY(@RestaurantIds)",
                        new
                        {
                            RestaurantIds = restaurants
                                .Select(x => x.Id)
                                .ToArray(),
                        });

                if (cuisines.Count() > 0)
                {
                    var map = restaurants.ToDictionary(x => x.Id);
                    foreach (var cuisine in cuisines)
                    {
                        map[cuisine.restaurant_id].Cuisines.Add(
                            new CuisineDto()
                            {
                                Name = cuisine.cuisine_name,
                            });
                    }
                }

                return restaurants;
            }
        }

        private RestaurantDto EntryToDto(RestaurantEntry entry)
        {
            return new RestaurantDto()
            {
                Id = entry.id,
                ManagerId = entry.manager_id,
                Name = entry.name,
                PhoneNumber = entry.phone_number,
                Address = entry.address,
                Latitude = entry.latitude,
                Longitude = entry.longitude,
                Status = entry.status,
                OpeningTimes = new OpeningTimesDto()
                {
                    Monday = entry.monday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.monday_open),
                        Close = FormatTimeSpan(entry.monday_close),
                    } : null,
                    Tuesday = entry.tuesday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.tuesday_open),
                        Close = FormatTimeSpan(entry.tuesday_close),
                    } : null,
                    Wednesday = entry.wednesday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.wednesday_open),
                        Close = FormatTimeSpan(entry.wednesday_close),
                    } : null,
                    Thursday = entry.thursday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.thursday_open),
                        Close = FormatTimeSpan(entry.thursday_close),
                    } : null,
                    Friday = entry.friday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.friday_open),
                        Close = FormatTimeSpan(entry.friday_close),
                    } : null,
                    Saturday = entry.saturday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.saturday_open),
                        Close = FormatTimeSpan(entry.saturday_close),
                    } : null,
                    Sunday = entry.sunday_open.HasValue ? new OpeningHoursDto()
                    {
                        Open = FormatTimeSpan(entry.sunday_open),
                        Close = FormatTimeSpan(entry.sunday_close),
                    } : null,
                },
                DeliveryFee = entry.delivery_fee,
                MinimumDeliverySpend = entry.minimum_delivery_spend,
                MaxDeliveryDistanceInKm = entry.max_delivery_distance_in_km,
                EstimatedDeliveryTimeInMinutes = entry.estimated_delivery_time_in_minutes,
            };
        }

        private string FormatTimeSpan(TimeSpan? t)
        {
            if (!t.HasValue)
            {
                return null;
            }

            return $"{t?.Hours.ToString().PadLeft(2, '0')}:{t?.Minutes.ToString().PadLeft(2, '0')}";
        }

        private record RestaurantEntry
        {
            public Guid id { get; init; }
            public Guid manager_id { get; init; }
            public string name { get; init; }
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
        }

        private record RestaurantCuisine
        {
            public Guid restaurant_id { get; init; }
            public string cuisine_name { get; init; }
        }
    }
}
