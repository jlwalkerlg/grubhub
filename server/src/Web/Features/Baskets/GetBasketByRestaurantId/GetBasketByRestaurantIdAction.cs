using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Services.Authentication;

namespace Web.Features.Baskets.GetBasketByRestaurantId
{
    public class GetBasketByRestaurantIdAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetBasketByRestaurantIdAction(
            IAuthenticator authenticator,
            IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [Authorize]
        [HttpGet("/restaurants/{restaurantId:guid}/basket")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var basket = await connection.QueryFirstOrDefaultAsync<BasketModel>(
                    @"SELECT
                            b.id,
                            b.user_id,
                            b.restaurant_id
                        FROM
                            baskets b
                        WHERE
                            b.user_id = @UserId
                            AND b.restaurant_id = @RestaurantId
                        ORDER BY b.id",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        RestaurantId = restaurantId,
                    });

            if (basket is null)
            {
                return StatusCode(200);
            }

            var items = await connection.QueryAsync<BasketItemModel>(
                    @"SELECT
                            bi.menu_item_id,
                            mi.name as menu_item_name,
                            mi.description as menu_item_description,
                            mi.price / 100.00 as menu_item_price,
                            bi.quantity
                        FROM
                            basket_items bi
                            INNER JOIN menu_items mi ON mi.id = bi.menu_item_id
                        WHERE
                            bi.basket_id = @BasketId",
                    new
                    {
                        BasketId = basket.Id,
                    });

            basket.Items = items.ToList();

            return Ok(basket);
        }

        public class BasketModel
        {
            [JsonIgnore] public int Id { get; init; }
            public Guid UserId { get; init; }
            public Guid RestaurantId { get; init; }
            public List<BasketItemModel> Items { get; set; }
        }

        public class BasketItemModel
        {
            public Guid MenuItemId { get; init; }
            public string MenuItemName { get; init; }
            public string MenuItemDescription { get; init; }
            public decimal MenuItemPrice { get; init; }
            public int Quantity { get; init; }
        }
    }
}
