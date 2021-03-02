using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Data;
using Web.Hubs;

namespace Web
{
    public class TestController : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IHubContext<OrderHub> hub;

        public TestController(IHubContext<OrderHub> hub, IDbConnectionFactory dbConnectionFactory)
        {
            this.hub = hub;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/test")]
        public async Task<IActionResult> Execute()
        {
            var orderId = Guid.NewGuid();

            using var connection = await dbConnectionFactory.OpenConnection();
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(@"
                INSERT INTO orders (
                    id,
                    user_id,
                    restaurant_id,
                    subtotal,
                    delivery_fee,
                    service_fee,
                    status,
                    mobile_number,
                    address,
                    placed_at,
                    confirmed_at,
                    payment_intent_id,
                    payment_intent_client_secret)
                VALUES (
                        @Id,
                        '979a79d6-7b7c-4c21-88c9-8f918be90d01',
                        '015caf13-8252-476b-9e7f-c43767998c01',
                        15,
                        2.49,
                        0.5,
                        'PaymentConfirmed',
                        '07454747488',
                        '19 Bodmin Avenue, Shipley, BD181LT',
                        @PlacedAt,
                        @ConfirmedAt,
                        '',
                        ''
                )",
                new
                {
                    Id = orderId,
                    PlacedAt = DateTime.UtcNow.AddSeconds(-10),
                    ConfirmedAt = DateTime.UtcNow,
                });

            await connection.ExecuteAsync(@"
                INSERT INTO order_items (menu_item_id, quantity, order_id)
                VALUES ('00a08447-d359-4c7b-80dc-ed3495985774', 2, @OrderId)",
                new
                {
                    OrderId = orderId,
                });

            transaction.Commit();

            await hub
                .Clients
                .Users("979a79d6-7b7c-4c21-88c9-8f918be90d01")
                .SendAsync($"new-order", orderId);

            return Ok();
        }
    }
}
