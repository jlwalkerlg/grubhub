using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Web.Data;

namespace Web.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IDistributedCache cache;

        public GetRestaurantByIdAction(
            IDbConnectionFactory dbConnectionFactory,
            IDistributedCache cache)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.cache = cache;
        }

        [HttpGet("/restaurants/{id:guid}")]
        public async Task<IActionResult> Execute([FromRoute] Guid id)
        {
            var restaurant = await GetFromCache(id);
            if (restaurant is not null) return Ok(restaurant);

            restaurant = await GetFromDatabase(id);
            if (restaurant is null) return NotFound("Restaurant not found.");

            await PutInCache(restaurant);

            return Ok(restaurant);
        }

        private async Task<RestaurantModel> GetFromCache(Guid id)
        {
            var json = await cache.GetStringAsync($"restaurant:{id}");
            return json is null ? null : JsonSerializer.Deserialize<RestaurantModel>(json);
        }

        private async Task PutInCache(RestaurantModel restaurant)
        {
            var json = JsonSerializer.Serialize(restaurant);
            await cache.SetStringAsync($"restaurant:{restaurant.Id}", json);
        }

        private async Task<RestaurantModel> GetFromDatabase(Guid id)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var restaurant = await connection
                .QuerySingleOrDefaultAsync<RestaurantDatabaseModel>(
                    @"SELECT
                        r.id,
                        r.manager_id,
                        r.name,
                        r.description,
                        r.phone_number,
                        r.address_line1,
                        r.address_line2,
                        r.city,
                        r.postcode,
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
                        r.delivery_fee / 100.00 as delivery_fee,
                        r.minimum_delivery_spend / 100.00 as minimum_delivery_spend,
                        r.max_delivery_distance_in_km,
                        r.estimated_delivery_time_in_minutes,
                        r.thumbnail,
                        r.banner
                    FROM
                        restaurants r
                    WHERE
                        r.id = @Id",
                    new { Id = id });

            if (restaurant is null) return null;

            var menu = await connection
                .QuerySingleOrDefaultAsync<MenuModel>(
                    @"SELECT
                            m.id,
                            m.restaurant_id
                        FROM
                            menus m
                        WHERE
                            m.restaurant_id = @RestaurantId",
                    new
                    {
                        RestaurantId = restaurant.Id,
                    });

            if (menu != null)
            {
                var categories = (await connection
                    .QueryAsync<MenuCategoryModel>(
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
                            MenuId = menu.Id,
                        })).ToList();

                var items = await connection
                    .QueryAsync<MenuItemModel>(
                        @"SELECT
                            i.id,
                            i.menu_category_id,
                            i.name,
                            i.description,
                            i.price / 100.00 as price
                        FROM
                            menu_items i
                        WHERE
                            i.menu_category_id = ANY(@CategoryIds)",
                        new
                        {
                            CategoryIds = categories
                                .Select(c => c.Id)
                                .ToArray(),
                        });

                var itemMapByCategoryId = items
                    .GroupBy(e => e.MenuCategoryId)
                    .ToDictionary(e => e.Key);

                foreach (var category in categories)
                {
                    if (itemMapByCategoryId.ContainsKey(category.Id))
                    {
                        category.Items = itemMapByCategoryId[category.Id].ToList();
                    }
                }

                menu.Categories = categories.ToList();
                restaurant.Menu = menu;
            }

            var cuisines = await connection
                .QueryAsync<CuisineModel>(
                    @"SELECT
                            rc.cuisine_name as name
                        FROM
                            restaurant_cuisines rc
                        WHERE
                            rc.restaurant_id = @RestaurantId",
                    new
                    {
                        RestaurantId = restaurant.Id,
                    });

            restaurant.Cuisines = cuisines.ToList();

            return restaurant.ToResponseModel();
        }

        private class RestaurantDatabaseModel
        {
            public Guid Id { get; init; }
            public Guid ManagerId { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }
            public string PhoneNumber { get; init; }
            public string AddressLine1 { get; init; }
            public string AddressLine2 { get; init; }
            public string City { get; init; }
            public string Postcode { get; init; }
            public float Latitude { get; init; }
            public float Longitude { get; init; }
            public string Status { get; init; }
            public TimeSpan? MondayOpen { get; init; }
            public TimeSpan? MondayClose { get; init; }
            public TimeSpan? TuesdayOpen { get; init; }
            public TimeSpan? TuesdayClose { get; init; }
            public TimeSpan? WednesdayOpen { get; init; }
            public TimeSpan? WednesdayClose { get; init; }
            public TimeSpan? ThursdayOpen { get; init; }
            public TimeSpan? ThursdayClose { get; init; }
            public TimeSpan? FridayOpen { get; init; }
            public TimeSpan? FridayClose { get; init; }
            public TimeSpan? SaturdayOpen { get; init; }
            public TimeSpan? SaturdayClose { get; init; }
            public TimeSpan? SundayOpen { get; init; }
            public TimeSpan? SundayClose { get; init; }
            public decimal DeliveryFee { get; init; }
            public decimal MinimumDeliverySpend { get; init; }
            public float MaxDeliveryDistanceInKm { get; init; }
            public int EstimatedDeliveryTimeInMinutes { get; init; }

            private readonly string thumbnail;
            public string Thumbnail
            {
                get => thumbnail == null
                    ? "https://d3bvhdd3xj1ghi.cloudfront.net/thumbnail.jpg"
                    : $"https://d3bvhdd3xj1ghi.cloudfront.net/restaurants/{Id}/{thumbnail}";
                init => thumbnail = value;
            }

            private readonly string banner;
            public string Banner
            {
                get => banner == null
                    ? "https://d3bvhdd3xj1ghi.cloudfront.net/banner.jpg"
                    : $"https://d3bvhdd3xj1ghi.cloudfront.net/restaurants/{Id}/{banner}";
                init => banner = value;
            }

            public List<CuisineModel> Cuisines { get; set; }
            public MenuModel Menu { get; set; }

            public RestaurantModel ToResponseModel()
            {
                return new()
                {
                    Id = Id,
                    ManagerId = ManagerId,
                    Name = Name,
                    Description = Description,
                    PhoneNumber = PhoneNumber,
                    AddressLine1 = AddressLine1,
                    AddressLine2 = AddressLine2,
                    City = City,
                    Postcode = Postcode,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Status = Status,
                    DeliveryFee = DeliveryFee,
                    MinimumDeliverySpend = MinimumDeliverySpend,
                    MaxDeliveryDistanceInKm = MaxDeliveryDistanceInKm,
                    EstimatedDeliveryTimeInMinutes = EstimatedDeliveryTimeInMinutes,
                    Thumbnail = Thumbnail,
                    Banner = Banner,
                    Cuisines = Cuisines,
                    Menu = Menu,
                    OpeningTimes = new OpeningTimesModel()
                    {
                        Monday = MondayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(MondayOpen),
                                Close = FormatTimeSpan(MondayClose),
                            }
                            : null,
                        Tuesday = TuesdayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(TuesdayOpen),
                                Close = FormatTimeSpan(TuesdayClose),
                            }
                            : null,
                        Wednesday = WednesdayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(WednesdayOpen),
                                Close = FormatTimeSpan(WednesdayClose),
                            }
                            : null,
                        Thursday = ThursdayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(ThursdayOpen),
                                Close = FormatTimeSpan(ThursdayClose),
                            }
                            : null,
                        Friday = FridayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(FridayOpen),
                                Close = FormatTimeSpan(FridayClose),
                            }
                            : null,
                        Saturday = SaturdayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(SaturdayOpen),
                                Close = FormatTimeSpan(SaturdayClose),
                            }
                            : null,
                        Sunday = SundayOpen.HasValue
                            ? new OpeningHoursModel()
                            {
                                Open = FormatTimeSpan(SundayOpen),
                                Close = FormatTimeSpan(SundayClose),
                            }
                            : null,
                    },
                };
            }

            private string FormatTimeSpan(TimeSpan? span)
            {
                return !span.HasValue
                    ? null
                    : $"{span?.Hours.ToString().PadLeft(2, '0')}:{span?.Minutes.ToString().PadLeft(2, '0')}";
            }
        }

        public class RestaurantModel
        {
            public Guid Id { get; init; }
            public Guid ManagerId { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }
            public string PhoneNumber { get; init; }
            public string AddressLine1 { get; init; }
            public string AddressLine2 { get; init; }
            public string City { get; init; }
            public string Postcode { get; init; }
            public float Latitude { get; init; }
            public float Longitude { get; init; }
            public string Status { get; init; }
            public OpeningTimesModel OpeningTimes { get; init; }
            public decimal DeliveryFee { get; init; }
            public decimal MinimumDeliverySpend { get; init; }
            public float MaxDeliveryDistanceInKm { get; init; }
            public int EstimatedDeliveryTimeInMinutes { get; init; }
            public string Thumbnail { get; init; }
            public string Banner { get; init; }
            public List<CuisineModel> Cuisines { get; init; }
            public MenuModel Menu { get; init; }
        }

        public class OpeningTimesModel
        {
            public OpeningHoursModel Monday { get; init; }
            public OpeningHoursModel Tuesday { get; init; }
            public OpeningHoursModel Wednesday { get; init; }
            public OpeningHoursModel Thursday { get; init; }
            public OpeningHoursModel Friday { get; init; }
            public OpeningHoursModel Saturday { get; init; }
            public OpeningHoursModel Sunday { get; init; }
        }

        public class OpeningHoursModel
        {
            public string Open { get; init; }
            public string Close { get; init; }
        }

        public class MenuModel
        {
            [JsonIgnore] public int Id { get; init; }
            public Guid RestaurantId { get; init; }
            public List<MenuCategoryModel> Categories { get; set; } = new();
        }

        public class MenuCategoryModel
        {
            public Guid Id { get; init; }
            public string Name { get; init; }
            public List<MenuItemModel> Items { get; set; } = new();
        }

        public class MenuItemModel
        {
            public Guid Id { get; init; }
            [JsonIgnore] public Guid MenuCategoryId { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }
            public decimal Price { get; init; }
        }

        public class CuisineModel
        {
            public string Name { get; init; }
        }
    }
}
