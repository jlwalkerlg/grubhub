using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application.Menus;

namespace FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Menus
{
    public class DPMenuDtoRepository : IMenuDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DPMenuDtoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<MenuDto> GetByRestaurantId(Guid restaurantId)
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

                await connection.QueryAsync<MenuEntry, MenuCategoryEntry, MenuItemEntry, object>(
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

                return menu;
            }
        }

        private class MenuEntry : MenuDto
        {
            public int Id { get; set; }
        }

        private class MenuCategoryEntry : MenuCategoryDto
        {
            public int Id { get; set; }
        }

        private class MenuItemEntry : MenuItemDto
        {
            public int Id { get; set; }
        }
    }
}
