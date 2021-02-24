using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Data.Models;

namespace Web.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetRestaurantByIdAction(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/restaurants/{id:guid}")]
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
                    .QuerySingleOrDefaultAsync<RestaurantModel>(
                        sql,
                        new { Id = id });

                if (restaurantEntry == null)
                {
                    return NotFound("Restaurant not found.");
                }

                var menuEntry = await connection
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
                            RestaurantId = restaurantEntry.id,
                        });

                if (menuEntry != null)
                {

                    var categoryEntries = await connection
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
                                MenuId = menuEntry.id,
                            });

                    var itemEntries = await connection
                        .QueryAsync<MenuItemModel>(
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
                    .QueryAsync<RestaurantCuisineModel>(
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
    }
}
