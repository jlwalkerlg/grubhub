using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Domain.Orders;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetActiveOrder
{
    public class GetActiveOrderAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetActiveOrderAction(
            IAuthenticator authenticator,
            IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/order")]
        public async Task<IActionResult> Execute()
        {
            if (!authenticator.IsAuthenticated)
            {
                return Unauthenticated();
            }

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var orderEntry = await connection
                    .QuerySingleOrDefaultAsync<OrderEntry>(
                    @"SELECT
                        o.id,
                        o.user_id,
                        o.restaurant_id,
                        o.status
                    FROM
                        orders o
                    WHERE
                        o.user_id = @UserId
                        AND o.status = @Status",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        Status = OrderStatus.Active.ToString(),
                    });

                if (orderEntry == null)
                {
                    return Ok<OrderDto>(null);
                }

                var orderItemEntries = await connection
                    .QueryAsync<OrderItemEntry>(
                        @"SELECT
                            oi.id,
                            oi.order_id,
                            oi.menu_item_id,
                            mi.name as menu_item_name,
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

        private record OrderEntry
        {
            public Guid id { get; init; }
            public Guid user_id { get; init; }
            public Guid restaurant_id { get; init; }
            public string status { get; init; }

            public OrderDto ToDto()
            {
                return new OrderDto()
                {
                    Id = id,
                    UserId = user_id,
                    RestaurantId = restaurant_id,
                    Status = status,
                };
            }
        }

        private record OrderItemEntry
        {
            public int id { get; init; }
            public Guid menu_item_id { get; init; }
            public string menu_item_name { get; init; }
            public decimal menu_item_price { get; init; }
            public int quantity { get; init; }
            public Guid order_id { get; init; }

            public OrderItemDto ToDto()
            {
                return new OrderItemDto()
                {
                    MenuItemId = menu_item_id,
                    MenuItemName = menu_item_name,
                    MenuItemPrice = menu_item_price,
                    Quantity = quantity,
                };
            }
        }
    }
}
