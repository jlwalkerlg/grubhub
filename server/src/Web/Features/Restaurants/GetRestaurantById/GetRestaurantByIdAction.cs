using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetRestaurantByIdAction(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/restaurants/{id}")]
        public async Task<IActionResult> Execute([FromRoute] Guid id)
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
                    r.id = @Id";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var entry = await connection
                    .QuerySingleOrDefaultAsync<RestaurantEntry>(
                        sql,
                        new { Id = id });

                if (entry == null)
                {
                    return Error(Web.Error.NotFound("Restaurant not found."));
                }

                var restaurant = EntryToDto(entry);

                var cuisines = await connection
                    .QueryAsync<RestaurantCuisine>(
                        @"SELECT
                            rc.restaurant_id,
                            rc.cuisine_name
                        FROM
                            restaurant_cuisines rc
                        WHERE
                            rc.restaurant_id = @RestaurantId",
                        new
                        {
                            RestaurantId = restaurant.Id,
                        });

                restaurant.Cuisines.AddRange(
                    cuisines.Select(x => new CuisineDto() { Name = x.cuisine_name })
                );

                return Ok(restaurant);
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

        private string FormatTimeSpan(TimeSpan? span)
        {
            if (!span.HasValue)
            {
                return null;
            }

            return $"{span?.Hours.ToString().PadLeft(2, '0')}:{span?.Minutes.ToString().PadLeft(2, '0')}";
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
