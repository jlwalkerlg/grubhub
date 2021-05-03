using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetOrderHistory
{
    public class GetOrderHistoryAction : Action
    {
        private const int PerPage = 15;

        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IAuthenticator authenticator;

        public GetOrderHistoryAction(IDbConnectionFactory dbConnectionFactory, IAuthenticator authenticator)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.authenticator = authenticator;
        }

        [Authorize]
        [HttpGet("/order-history")]
        public async Task<IActionResult> GetOrderHistory([FromQuery] int? page)
        {
            page = Math.Max(page ?? 1, 1);
            var offset = (page - 1) * PerPage;

            using var connection = await dbConnectionFactory.OpenConnection();

            var orders = await connection.QueryAsync<OrderModel>(
                    @"SELECT
                        o.id,
                        o.placed_at,
                        SUM(oi.quantity) as total_items,
                        SUM(oi.price * oi.quantity) / 100.00 as subtotal,
                        o.service_fee / 100.00 as service_fee,
                        o.delivery_fee / 100.00 as delivery_fee,
                        r.name as restaurant_name,
                        r.thumbnail as restaurant_thumbnail
                    FROM
                        orders o
                        INNER JOIN order_items oi on o.id = oi.order_id
                        INNER JOIN restaurants r on o.restaurant_id = r.id
                    WHERE
                        o.user_id = @UserId
                    GROUP BY
                        o.id,
                        o.placed_at,
                        o.service_fee,
                        o.delivery_fee,
                        r.name,
                        r.thumbnail,
                        o.delivered_at
                    ORDER BY o.delivered_at
                    LIMIT @Limit OFFSET @Offset",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        Offset = offset,
                        Limit = PerPage,
                    });

            var count = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM orders o WHERE o.user_id = @UserId",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                    });

            return Ok(new GetOrderHistoryResponse()
            {
                Orders = orders,
                Count = count,
            });
        }

        public record OrderModel
        {
            public string Id { get; init; }
            public DateTimeOffset PlacedAt { get; init; }
            public int TotalItems { get; init; }
            public decimal Subtotal { get; init; }
            public decimal ServiceFee { get; init; }
            public decimal DeliveryFee { get; init; }
            public string RestaurantName { get; init; }

            private readonly string restaurantThumbnail;
            public string RestaurantThumbnail
            {
                get => restaurantThumbnail == null
                    ? "https://d3bvhdd3xj1ghi.cloudfront.net/thumbnail.jpg"
                    : $"https://d3bvhdd3xj1ghi.cloudfront.net/restaurants/{Id}/{restaurantThumbnail}";
                init => restaurantThumbnail = value;
            }
        }

        public record GetOrderHistoryResponse
        {
            public IEnumerable<OrderModel> Orders { get; init; }
            public int Count { get; init; }
        }
    }
}
