using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Domain.Orders;
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

        [Authorize]
        [HttpGet("/restaurant/order-history")]
        public async Task<IActionResult> GetRestaurantOrderHistory()
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var orders = await connection
                .QueryAsync<OrderModel>(
                    @"
                        SELECT
                            o.id,
                            o.number,
                            o.status,
                            o.placed_at,
                            o.delivered_at,
                            SUM(oi.price * oi.quantity) as subtotal
                        FROM
                            orders o
                            INNER JOIN order_items oi ON o.id = oi.order_id
                            INNER JOIN restaurants r ON r.id = o.restaurant_id
                        WHERE
                            r.manager_id = @UserId
                            AND o.status = ANY(@InactiveStatuses)
                        GROUP BY o.id, r.estimated_delivery_time_in_minutes
                        ORDER BY o.placed_at",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        InactiveStatuses = (new[] {OrderStatus.Delivered, OrderStatus.Rejected})
                            .Select(x => x.ToString())
                            .ToArray(),
                    });

            return Ok(orders);
        }

        public record OrderModel
        {
            public string Id { get; set; }
            public int Number { get; set; }
            public string Status { get; set; }
            public DateTime PlacedAt { get; set; }
            public DateTime? DeliveredAt { get; set; }
            public decimal Subtotal { get; set; }
        }
    }
}
