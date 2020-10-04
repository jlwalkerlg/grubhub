using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Domain;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Actions.Restaurants.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdHandler : IRequestHandler<GetMenuByRestaurantIdQuery, MenuDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetMenuByRestaurantIdHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<MenuDto>> Handle(GetMenuByRestaurantIdQuery query, CancellationToken cancellationToken)
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

                await connection.QueryAsync<MenuDto, MenuCategoryDto, MenuItemDto, object>(
                    sql,
                    (menuEntry, categoryEntry, item) =>
                    {
                        if (menu == null)
                        {
                            menu = menuEntry;
                        }

                        if (categoryEntry != null)
                        {
                            var category = menu.Categories.FirstOrDefault(x => x.Name == categoryEntry.Name);

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
                    new { RestaurantId = query.RestaurantId });

                return Result.Ok(menu);
            }
        }
    }
}
