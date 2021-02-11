using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Features.Orders.GetActiveOrder;
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

        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> Execute([FromRoute] string id)
        {
            if (!authenticator.IsAuthenticated)
            {
                return Unauthenticated();
            }

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var orderEntry = await connection
                    .QueryFirstOrDefaultAsync<OrderEntry>(
                        @"SELECT
                            o.id,
                            o.user_id,
                            o.restaurant_id,
                            subtotal,
                            delivery_fee,
                            service_fee,
                            o.status,
                            o.address,
                            o.placed_at
                        FROM
                            orders o
                        WHERE
                            o.id = @Id
                        ORDER BY o.id",
                        new
                        {
                            Id = id,
                        });

                if (orderEntry == null)
                {
                    return NotFound();
                }

                if (orderEntry.user_id != authenticator.UserId)
                {
                    return Unauthorised();
                }

                var orderItemEntries = await connection
                    .QueryAsync<OrderItemEntry>(
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

        private record OrderEntry
        {
            public string id { get; init; }
            public Guid user_id { get; init; }
            public Guid restaurant_id { get; init; }
            public decimal subtotal { get; init; }
            public decimal delivery_fee { get; init; }
            public decimal service_fee { get; init; }
            public string status { get; init; }
            public string address { get; init; }
            public DateTime placed_at { get; init; }

            public OrderDto ToDto()
            {
                return new OrderDto()
                {
                    Id = id,
                    UserId = user_id,
                    RestaurantId = restaurant_id,
                    Subtotal = subtotal,
                    DeliveryFee = delivery_fee,
                    ServiceFee = service_fee,
                    Status = status,
                    Address = address,
                    PlacedAt = placed_at,
                };
            }
        }

        private record OrderItemEntry
        {
            public int id { get; init; }
            public Guid menu_item_id { get; init; }
            public string menu_item_name { get; init; }
            public string menu_item_description { get; init; }
            public decimal menu_item_price { get; init; }
            public int quantity { get; init; }

            public OrderItemDto ToDto()
            {
                return new OrderItemDto()
                {
                    MenuItemId = menu_item_id,
                    MenuItemName = menu_item_name,
                    MenuItemDescription = menu_item_description,
                    MenuItemPrice = menu_item_price,
                    Quantity = quantity,
                };
            }
        }
    }
}
