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
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IAuthenticator authenticator;

        public GetOrderHistoryAction(IDbConnectionFactory dbConnectionFactory, IAuthenticator authenticator)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.authenticator = authenticator;
        }

        [Authorize]
        [HttpGet("/order-history")]
        public async Task<IActionResult> GetOrderHistory([FromQuery] int? page, [FromQuery] int? perPage)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var sql = @"SELECT
                    o.id,
                    o.placed_at,
                    SUM(oi.quantity) as total_items,
                    SUM(oi.price * oi.quantity) / 100.00 as subtotal,
                    o.service_fee / 100.00 as service_fee,
                    o.delivery_fee / 100.00 as delivery_fee,
                    r.name as restaurant_name
                FROM
                    orders o
                    INNER JOIN order_items oi on o.id = oi.order_id
                    INNER JOIN restaurants r on o.restaurant_id = r.id
                WHERE
                    o.user_id = @UserId
                GROUP BY o.id, r.name
                ORDER BY o.delivered_at";

            int? offset = null;

            if (perPage > 0)
            {
                perPage = Math.Max(perPage.Value, 0);

                var currentPage = Math.Max(page ?? 1, 1);
                offset = (currentPage - 1) * perPage;

                sql += " LIMIT @Limit OFFSET @Offset";
            }

            var orders = await connection
                .QueryAsync<OrderModel>(
                    sql,
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        Offset = offset,
                        Limit = perPage,
                    });

            var count = await connection
                .ExecuteScalarAsync<int>(
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
            public string Id { get; set; }
            public DateTime PlacedAt { get; set; }
            public int TotalItems { get; set; }
            public decimal Subtotal { get; set; }
            public decimal ServiceFee { get; set; }
            public decimal DeliveryFee { get; set; }
            public string RestaurantName { get; set; }
        }

        public record GetOrderHistoryResponse
        {
            public IEnumerable<OrderModel> Orders { get; init; }
            public int Count { get; init; }
        }
    }
}
