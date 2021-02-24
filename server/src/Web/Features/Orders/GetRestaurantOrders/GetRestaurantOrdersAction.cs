using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Data.Models;
using Web.Domain.Users;
using Web.Features.Orders.GetActiveOrder;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetRestaurantOrders
{
    public class GetRestaurantOrdersAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetRestaurantOrdersAction(IAuthenticator authenticator, IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurant/orders")]
        public async Task<IActionResult> Execute()
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var orderEntries = await connection
                .QueryAsync<OrderModel>(
                    @"
                        SELECT
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
                            r.name AS restaurant_name,
                            r.address AS restaurant_address,
                            r.phone_number as restaurant_phone_number
                        FROM
                            orders o
                            INNER JOIN restaurants r on o.restaurant_id = r.id
                        WHERE
                            r.manager_id = @UserId
                            ORDER BY o.placed_at",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                    });

            var orders = orderEntries.Select(x => x.ToDto()).ToArray();

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
                            oi.order_id = Any(@OrderIds)",
                    new
                    {
                        OrderIds = orders.Select(x => x.Id).ToArray(),
                    });

            var ordersMap = orders.ToDictionary(x => x.Id);

            foreach (var item in orderItemEntries)
            {
                ordersMap[item.order_id].Items.Add(item.ToDto());
            }

            return Ok(orders);
        }
    }
}
