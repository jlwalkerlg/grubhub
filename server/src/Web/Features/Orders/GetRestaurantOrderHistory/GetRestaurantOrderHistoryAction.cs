using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetRestaurantOrderHistory
{
    public class GetRestaurantOrderHistoryAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IAuthenticator authenticator;

        public GetRestaurantOrderHistoryAction(IDbConnectionFactory dbConnectionFactory, IAuthenticator authenticator)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.authenticator = authenticator;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurant/order-history")]
        public async Task<IActionResult> GetRestaurantOrderHistory([FromQuery] int? page, [FromQuery] int? perPage)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var sql = @"SELECT
                    o.id,
                    o.number,
                    o.status,
                    o.placed_at,
                    SUM(oi.price * oi.quantity) / 100.00 as subtotal
                FROM
                    orders o
                    INNER JOIN order_items oi ON o.id = oi.order_id
                    INNER JOIN restaurants r ON r.id = o.restaurant_id
                WHERE
                    r.manager_id = @UserId
                    AND o.status = ANY(@InactiveStatuses)
                GROUP BY o.id, r.estimated_delivery_time_in_minutes
                ORDER BY o.placed_at";

            int? offset = null;

            if (perPage > 0)
            {
                perPage = Math.Max(perPage.Value, 0);

                var currentPage = Math.Max(page ?? 1, 1);
                offset = (currentPage - 1) * perPage;

                sql += " LIMIT @Limit OFFSET @Offset";
            }

            var orders = await connection.QueryAsync<OrderModel>(
                    sql,
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        InactiveStatuses = (new[] {OrderStatus.Delivered, OrderStatus.Rejected, OrderStatus.Cancelled})
                            .Select(x => x.ToString())
                            .ToArray(),
                        Offset = offset,
                        Limit = perPage,
                    });

            var count = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*)
                        FROM orders o
                        INNER JOIN restaurants r ON r.id = o.restaurant_id
                        WHERE r.manager_id = @UserId
                        AND o.status = ANY(@InactiveStatuses)",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        InactiveStatuses = (new[] {OrderStatus.Delivered, OrderStatus.Rejected, OrderStatus.Cancelled})
                            .Select(x => x.ToString())
                            .ToArray(),
                    });

            return Ok(new GetRestaurantOrderHistoryResponse()
            {
                Orders = orders,
                Count = count,
            });
        }

        public class OrderModel
        {
            public string Id { get; init; }
            public int Number { get; init; }
            public string Status { get; init; }
            public DateTime PlacedAt { get; init; }
            public decimal Subtotal { get; init; }
        }

        public class GetRestaurantOrderHistoryResponse
        {
            public IEnumerable<OrderModel> Orders { get; init; }
            public int Count { get; init; }
        }
    }
}
