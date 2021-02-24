using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Data.Models;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetOrderById
{
    public class GetOrderByIdAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetOrderByIdAction(
            IAuthenticator authenticator,
            IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [Authorize]
        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> Execute([FromRoute] string id)
        {
            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var orderEntry = await connection
                    .QueryFirstOrDefaultAsync<OrderModel>(
                        @"SELECT
                            o.id,
                            o.number,
                            o.user_id,
                            o.restaurant_id,
                            o.subtotal,
                            o.delivery_fee,
                            o.service_fee,
                            o.status,
                            o.address,
                            o.placed_at,
                            o.payment_intent_client_secret,
                            r.name AS restaurant_name,
                            r.address AS restaurant_address,
                            r.phone_number as restaurant_phone_number
                        FROM
                            orders o
                            INNER JOIN restaurants r ON r.id = o.restaurant_id
                        WHERE
                            o.id = @Id
                        ORDER BY o.id",
                        new
                        {
                            Id = id,
                        });

                if (orderEntry is null)
                {
                    return NotFound();
                }

                if (orderEntry.user_id != authenticator.UserId)
                {
                    return Unauthorised();
                }

                var orderItemEntries = await connection
                    .QueryAsync<OrderItemModel>(
                        @"SELECT
                            oi.id,
                            oi.order_id,
                            oi.menu_item_id,
                            mi.name as menu_item_name,
                            mi.description as menu_item_description,
                            mi.price as menu_item_price,
                            oi.quantity
                        FROM
                            order_items oi
                            INNER JOIN menu_items mi ON mi.id = oi.menu_item_id
                        WHERE
                            oi.order_id = @OrderId",
                        new
                        {
                            OrderId = orderEntry.id,
                        });

                var order = orderEntry.ToDto();
                order.Items.AddRange(orderItemEntries.Select(x => x.ToDto()));

                return Ok(order);
            }
        }
    }
}
