using System;
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
        public async Task<IActionResult> GetOrderHistory()
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var orders = await connection
                .QueryAsync<OrderModel>(
                    @"
                        SELECT
                            o.id,
                            o.placed_at,
                            SUM(oi.quantity) as total_items,
                            SUM(oi.price * oi.quantity) as subtotal,
                            o.service_fee,
                            o.delivery_fee,
                            r.name as restaurant_name
                        FROM
                            orders o
                            INNER JOIN order_items oi on o.id = oi.order_id
                            INNER JOIN restaurants r on o.restaurant_id = r.id
                        WHERE
                            o.user_id = @UserId
                        GROUP BY o.id, r.name
                        ORDER BY o.delivered_at",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                    });

            return Ok(orders);
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
    }
}
