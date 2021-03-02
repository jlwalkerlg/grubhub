using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Data.Models;
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
                            o.subtotal,
                            o.status,
                            o.address,
                            o.placed_at,
                            r.estimated_delivery_time_in_minutes
                        FROM
                            orders o
                            INNER JOIN restaurants r ON r.manager_id = o.user_id
                        WHERE
                            r.manager_id = @UserId
                            AND o.confirmed_at > @ConfirmedAfter
                        ORDER BY o.confirmed_at",
                    new
                    {
                        UserId = authenticator.UserId.Value,
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
            public string Address { get; set; }
            public DateTime PlacedAt { get; set; }
            [JsonIgnore]
            public int EstimatedDeliveryTimeInMinutes { get; set; }
            public DateTime EstimatedDeliveryTime => PlacedAt.AddMinutes(EstimatedDeliveryTimeInMinutes);
        }
    }
}
