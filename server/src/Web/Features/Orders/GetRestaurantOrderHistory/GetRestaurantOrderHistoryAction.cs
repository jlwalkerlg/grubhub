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
        private const int PerPage = 2;

        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IAuthenticator authenticator;

        public GetRestaurantOrderHistoryAction(IDbConnectionFactory dbConnectionFactory, IAuthenticator authenticator)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.authenticator = authenticator;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurant/order-history")]
        public async Task<IActionResult> GetRestaurantOrderHistory([FromQuery] int? page)
        {
            page = Math.Max(page ?? 1, 1);
            var offset = (page - 1) * PerPage;

            using var connection = await dbConnectionFactory.OpenConnection();

            var orders = await connection.QueryAsync<OrderModel>(
                    @"SELECT
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
                    GROUP BY
                        o.id,
                        o.number,
                        o.status,
                        o.placed_at
                    ORDER BY o.placed_at
                    LIMIT @Limit OFFSET @Offset",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        InactiveStatuses = (new[] { OrderStatus.Delivered, OrderStatus.Rejected, OrderStatus.Cancelled })
                            .Select(x => x.ToString())
                            .ToArray(),
                        Offset = offset,
                        Limit = PerPage,
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
                        InactiveStatuses = (new[] { OrderStatus.Delivered, OrderStatus.Rejected, OrderStatus.Cancelled })
                            .Select(x => x.ToString())
                            .ToArray(),
                    });

            var pages = (int)Math.Ceiling((double)count / PerPage);

            return Ok(new GetRestaurantOrderHistoryResponse()
            {
                Orders = orders,
                Pages = Math.Max(1, pages),
            });
        }

        public class OrderModel
        {
            public string Id { get; init; }
            public int Number { get; init; }
            public string Status { get; init; }
            public DateTimeOffset PlacedAt { get; init; }
            public decimal Subtotal { get; init; }
        }

        public class GetRestaurantOrderHistoryResponse
        {
            public IEnumerable<OrderModel> Orders { get; init; }
            public int Pages { get; init; }
        }
    }
}
