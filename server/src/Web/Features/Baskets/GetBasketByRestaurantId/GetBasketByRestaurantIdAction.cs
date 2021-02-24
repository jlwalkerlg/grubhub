using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Data.Models;
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
            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var basketEntry = await connection
                    .QueryFirstOrDefaultAsync<BasketEntry>(
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

                if (basketEntry == null)
                {
                    return StatusCode(200);
                }

                var basketItemEntries = await connection
                    .QueryAsync<BasketItemEntry>(
                        @"SELECT
                            bi.id,
                            bi.basket_id,
                            bi.menu_item_id,
                            mi.name as menu_item_name,
                            mi.description as menu_item_description,
                            mi.price as menu_item_price,
                            bi.quantity
                        FROM
                            basket_items bi
                            INNER JOIN menu_items mi ON mi.id = bi.menu_item_id
                        WHERE
                            bi.basket_id = @BasketId",
                        new
                        {
                            BasketId = basketEntry.id,
                        });

                var basket = basketEntry.ToDto();
                basket.Items.AddRange(basketItemEntries.Select(x => x.ToDto()));

                return Ok(basket);
            }
        }
    }
}
