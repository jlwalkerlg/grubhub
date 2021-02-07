using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Features.Cuisines;
using Web.Features.Menus;

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
                    r.description,
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
                var restaurantEntry = await connection
                    .QuerySingleOrDefaultAsync<RestaurantEntry>(
                        sql,
                        new { Id = id });

                if (restaurantEntry == null)
                {
                    return NotFound("Restaurant not found.");
                }

                var menuEntry = await connection
                    .QuerySingleOrDefaultAsync<MenuEntry>(
                        @"SELECT
                            m.id,
                            m.restaurant_id
                        FROM
                            menus m
                        WHERE
                            m.restaurant_id = @RestaurantId",
                        new
                        {
                            RestaurantId = restaurantEntry.id,
                        });

                if (menuEntry != null)
                {

                    var categoryEntries = await connection
                        .QueryAsync<MenuCategoryEntry>(
                            @"SELECT
                            mc.id,
                            mc.menu_id,
                            mc.name
                        FROM
                            menu_categories mc
                        WHERE
                            mc.menu_id = @MenuId",
                            new
                            {
                                MenuId = menuEntry.id,
                            });

                    var itemEntries = await connection
                        .QueryAsync<MenuItemEntry>(
                            @"SELECT
                            i.id,
                            i.menu_category_id,
                            i.name,
                            i.description,
                            i.price
                        FROM
                            menu_items i
                        WHERE
                            i.menu_category_id = ANY(@CategoryIds)",
                            new
                            {
                                CategoryIds = categoryEntries
                                    .Select(c => c.id)
                                    .ToArray(),
                            });

                    var menu = menuEntry.ToDto();

                    var itemMapByCategoryId = itemEntries
                    .GroupBy(e => e.menu_category_id)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Select(e => e.ToDto()));

                    foreach (var categoryEntry in categoryEntries)
                    {
                        var category = categoryEntry.ToDto();

                        if (itemMapByCategoryId.ContainsKey(categoryEntry.id))
                        {
                            category.Items.AddRange(itemMapByCategoryId[categoryEntry.id]);
                        }

                        menu.Categories.Add(category);
                    }

                    restaurantEntry.Menu = menu;
                }

                var cuisineEntries = await connection
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
                            RestaurantId = restaurantEntry.id,
                        });

                var restaurant = restaurantEntry.ToDto();

                restaurant.Cuisines.AddRange(cuisineEntries.Select(x => x.ToCuisineDto()));

                return Ok(restaurant);
            }
        }

        private record RestaurantEntry
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
                return new RestaurantDto()
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
                        Monday = monday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(monday_open),
                            Close = FormatTimeSpan(monday_close),
                        } : null,
                        Tuesday = tuesday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(tuesday_open),
                            Close = FormatTimeSpan(tuesday_close),
                        } : null,
                        Wednesday = wednesday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(wednesday_open),
                            Close = FormatTimeSpan(wednesday_close),
                        } : null,
                        Thursday = thursday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(thursday_open),
                            Close = FormatTimeSpan(thursday_close),
                        } : null,
                        Friday = friday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(friday_open),
                            Close = FormatTimeSpan(friday_close),
                        } : null,
                        Saturday = saturday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(saturday_open),
                            Close = FormatTimeSpan(saturday_close),
                        } : null,
                        Sunday = sunday_open.HasValue ? new OpeningHoursDto()
                        {
                            Open = FormatTimeSpan(sunday_open),
                            Close = FormatTimeSpan(sunday_close),
                        } : null,
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
                if (!span.HasValue)
                {
                    return null;
                }

                return $"{span?.Hours.ToString().PadLeft(2, '0')}:{span?.Minutes.ToString().PadLeft(2, '0')}";
            }
        }

        private record MenuEntry
        {
            public int id { get; init; }
            public Guid restaurant_id { get; init; }

            public MenuDto ToDto()
            {
                return new MenuDto()
                {
                    RestaurantId = restaurant_id,
                };
            }
        }

        private record MenuCategoryEntry
        {
            public Guid id { get; init; }
            public int menu_id { get; init; }
            public string name { get; init; }

            public MenuCategoryDto ToDto()
            {
                return new MenuCategoryDto()
                {
                    Id = id,
                    Name = name,
                };
            }
        }

        private record MenuItemEntry
        {
            public Guid id { get; init; }
            public Guid menu_category_id { get; init; }
            public string name { get; init; }
            public string description { get; init; }
            public decimal price { get; init; }

            public MenuItemDto ToDto()
            {
                return new MenuItemDto()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Price = price,
                };
            }
        }

        private record RestaurantCuisine
        {
            public string cuisine_name { get; init; }

            public CuisineDto ToCuisineDto()
            {
                return new CuisineDto()
                {
                    Name = cuisine_name,
                };
            }
        }
    }
}
