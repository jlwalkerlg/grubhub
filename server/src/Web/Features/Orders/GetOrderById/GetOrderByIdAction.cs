using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
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

        [Authorize]
        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> Execute([FromRoute] string id)
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var order = await connection
                .QueryFirstOrDefaultAsync<OrderModel>(
                    @"SELECT
                            o.id,
                            o.number,
                            o.user_id,
                            o.restaurant_id,
                            o.delivery_fee,
                            o.service_fee,
                            o.status,
                            o.address_line1,
                            o.address_line2,
                            o.city,
                            o.postcode,
                            o.placed_at,
                            o.confirmed_at,
                            o.accepted_at,
                            o.rejected_at,
                            o.delivered_at,
                            o.cancelled_at,
                            o.payment_intent_client_secret,
                            r.name AS restaurant_name,
                            r.address_line1 AS restaurant_address_line1,
                            r.address_line2 AS restaurant_address_line2,
                            r.city AS restaurant_city,
                            r.postcode AS restaurant_postcode,
                            r.phone_number as restaurant_phone_number,
                            r.estimated_delivery_time_in_minutes,
                            u.name as customer_name,
                            u.email as customer_email,
                            o.mobile_number as customer_mobile
                        FROM
                            orders o
                            INNER JOIN restaurants r ON r.id = o.restaurant_id
                            INNER JOIN users u ON u.id = o.user_id
                        WHERE
                            o.id = @Id
                        ORDER BY o.id",
                    new
                    {
                        Id = id,
                    });

            if (order is null)
            {
                return NotFound();
            }

            if (order.UserId != authenticator.UserId)
            {
                return Unauthorised();
            }

            var items = await connection
                .QueryAsync<OrderItemModel>(
                    @"SELECT
                            oi.id,
                            oi.name,
                            oi.price,
                            oi.quantity
                        FROM
                            order_items oi
                        WHERE
                            oi.order_id = @OrderId",
                    new
                    {
                        OrderId = order.Id,
                    });

            order.Items = items.ToList();

            return Ok(order);
        }

        public record OrderModel
        {
            public string Id { get; set; }
            public int Number { get; set; }
            public Guid UserId { get; set; }
            public Guid RestaurantId { get; set; }
            public decimal DeliveryFee { get; set; }
            public decimal ServiceFee { get; set; }
            public string Status { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public DateTime PlacedAt { get; set; }
            public DateTime? ConfirmedAt { get; set; }
            public DateTime? AcceptedAt { get; set; }
            public DateTime? RejectedAt { get; set; }
            public DateTime? DeliveredAt { get; set; }
            public DateTime? CancelledAt { get; set; }
            public string PaymentIntentClientSecret { get; set; }
            public string RestaurantName { get; set; }
            public string RestaurantAddressLine1 { get; set; }
            public string RestaurantAddressLine2 { get; set; }
            public string RestaurantCity { get; set; }
            public string RestaurantPostcode { get; set; }
            public string RestaurantPhoneNumber { get; set; }
            [JsonIgnore]
            public int EstimatedDeliveryTimeInMinutes { get; set; }
            public DateTime EstimatedDeliveryTime => PlacedAt.AddMinutes(EstimatedDeliveryTimeInMinutes);
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerMobile { get; set; }

            public List<OrderItemModel> Items { get; set; }
            public decimal Subtotal => Items.Aggregate(
                0m,
                (acc, item) => acc + item.Price * item.Quantity);
        }

        public record OrderItemModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
