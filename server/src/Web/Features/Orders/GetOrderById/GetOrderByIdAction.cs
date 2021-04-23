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
                            o.delivery_fee / 100.00 as delivery_fee,
                            o.service_fee / 100.00 as service_fee,
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
                            u.first_name as customer_first_name,
                            u.last_name as customer_last_name,
                            u.email as customer_email,
                            o.mobile_number as customer_mobile,
                            r.manager_id as restaurant_manager_id,
                            r.thumbnail as restaurant_thumbnail
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

            if (order.UserId != authenticator.UserId && authenticator.UserId != order.RestaurantManagerId)
            {
                return Unauthorised();
            }

            var items = await connection.QueryAsync<OrderItemModel>(
                    @"SELECT
                            oi.id,
                            oi.name,
                            oi.price / 100.00 as price,
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

        public class OrderModel
        {
            public string Id { get; init; }
            public int Number { get; init; }
            public Guid UserId { get; init; }
            public Guid RestaurantId { get; init; }
            public decimal DeliveryFee { get; init; }
            public decimal ServiceFee { get; init; }
            public string Status { get; init; }
            public string AddressLine1 { get; init; }
            public string AddressLine2 { get; init; }
            public string City { get; init; }
            public string Postcode { get; init; }
            public DateTimeOffset PlacedAt { get; init; }
            public DateTimeOffset? ConfirmedAt { get; init; }
            public DateTimeOffset? AcceptedAt { get; init; }
            public DateTimeOffset? RejectedAt { get; init; }
            public DateTimeOffset? DeliveredAt { get; init; }
            public DateTimeOffset? CancelledAt { get; init; }
            public string PaymentIntentClientSecret { get; init; }
            public string RestaurantName { get; init; }
            public string RestaurantAddressLine1 { get; init; }
            public string RestaurantAddressLine2 { get; init; }
            public string RestaurantCity { get; init; }
            public string RestaurantPostcode { get; init; }
            public string RestaurantPhoneNumber { get; init; }
            [JsonIgnore] public int EstimatedDeliveryTimeInMinutes { get; init; }
            public DateTimeOffset EstimatedDeliveryTime => PlacedAt.AddMinutes(EstimatedDeliveryTimeInMinutes);
            public string CustomerFirstName { get; init; }
            public string CustomerLastName { get; init; }
            public string CustomerEmail { get; init; }
            public string CustomerMobile { get; init; }
            [JsonIgnore] public Guid RestaurantManagerId { get; init; }

            private readonly string restaurantThumbnail;
            public string RestaurantThumbnail
            {
                get => restaurantThumbnail == null
                    ? "https://d3bvhdd3xj1ghi.cloudfront.net/thumbnail.jpg"
                    : $"https://d3bvhdd3xj1ghi.cloudfront.net/restaurants/{Id}/{restaurantThumbnail}";
                init => restaurantThumbnail = value;
            }

            public List<OrderItemModel> Items { get; set; }
            public decimal Subtotal => Items.Aggregate(0m,
                (acc, item) => acc + item.Price * item.Quantity);
        }

        public class OrderItemModel
        {
            public int Id { get; init; }
            public string Name { get; init; }
            public decimal Price { get; init; }
            public int Quantity { get; init; }
        }
    }
}
