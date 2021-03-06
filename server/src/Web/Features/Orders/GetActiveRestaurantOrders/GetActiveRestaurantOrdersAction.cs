using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Services.Authentication;

namespace Web.Features.Orders.GetActiveRestaurantOrders
{
    public class GetActiveRestaurantOrdersAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetActiveRestaurantOrdersAction(IAuthenticator authenticator, IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurant/active-orders")]
        public async Task<IActionResult> Execute([FromQuery] DateTime confirmedAfter = default)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var orders = await connection
                .QueryAsync<OrderModel>(
                    @"
                        SELECT
                            o.id,
                            o.number,
                            o.status,
                            o.address_line1,
                            o.address_line2,
                            o.city,
                            o.postcode,
                            o.placed_at,
                            r.estimated_delivery_time_in_minutes,
                            SUM(oi.price * oi.quantity) as subtotal
                        FROM
                            orders o
                            INNER JOIN order_items oi ON o.id = oi.order_id
                            INNER JOIN restaurants r ON r.id = o.restaurant_id
                        WHERE
                            r.manager_id = @UserId
                            AND o.status = ANY(@ActiveStatuses)
                            AND o.confirmed_at > @ConfirmedAfter
                        GROUP BY o.id, r.estimated_delivery_time_in_minutes
                        ORDER BY o.confirmed_at",
                    new
                    {
                        UserId = authenticator.UserId.Value,
                        ActiveStatuses = (new[] {OrderStatus.PaymentConfirmed, OrderStatus.Accepted})
                            .Select(x => x.ToString())
                            .ToArray(),
                        ConfirmedAfter = confirmedAfter,
                    });

            return Ok(orders);
        }

        public record OrderModel
        {
            public string Id { get; set; }
            public int Number { get; set; }
            public decimal Subtotal { get; set; }
            public string Status { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public DateTime PlacedAt { get; set; }
            [JsonIgnore]
            public int EstimatedDeliveryTimeInMinutes { get; set; }
            public DateTime EstimatedDeliveryTime => PlacedAt.AddMinutes(EstimatedDeliveryTimeInMinutes);
        }
    }
}
