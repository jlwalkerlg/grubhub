using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetMenuByRestaurantIdAction(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/restaurants/{restaurantId}/menu")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var sql = @"
                SELECT
                    m.id,
                    m.restaurant_id,
                    c.id,
                    c.name,
                    i.id,
                    i.name,
                    i.description,
                    i.price
                FROM
                    menus m
                LEFT JOIN menu_categories c ON c.menu_id = m.id
                LEFT JOIN menu_items i ON i.menu_category_id = c.id
                WHERE
                    m.restaurant_id = @RestaurantId";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                MenuDto menu = null;

                await connection.QueryAsync<MenuEntry, MenuCategoryDto, MenuItemDto, object>(
                    sql,
                    (menuEntry, categoryEntry, item) =>
                    {
                        if (menu == null)
                        {
                            menu = menuEntry;
                        }

                        if (categoryEntry != null)
                        {
                            var category = menu.Categories
                                .SingleOrDefault(x => x.Name == categoryEntry.Name);

                            if (category == null)
                            {
                                category = categoryEntry;
                                menu.Categories.Add(category);
                            }

                            if (item != null)
                            {
                                category.Items.Add(item);
                            }
                        }

                        return null;
                    },
                    new { RestaurantId = restaurantId });

                return menu == null
                    ? NotFound("Menu not found.")
                    : Ok(menu);
            }
        }

        private record MenuEntry : MenuDto
        {
            public int Id { get; set; }
        }
    }
}
