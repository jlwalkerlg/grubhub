using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Services.Authentication;

namespace Web.Features.Billing.GetBillingDetails
{
    public class GetBillingDetailsAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetBillingDetailsAction(IAuthenticator authenticator, IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/restaurants/{restaurantId:guid}/billing")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            if (!authenticator.IsAuthenticated)
            {
                return Unauthenticated();
            }

            var sql = @"
                SELECT
                    ba.id,
                    ba.restaurant_id,
                    ba.billing_enabled,
                    r.manager_id
                FROM
                    billing_accounts ba
                    INNER JOIN restaurants r ON r.id = ba.restaurant_id
                WHERE
                    ba.restaurant_id = @RestaurantId";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var billingDetailsEntry = await connection
                    .QuerySingleOrDefaultAsync<BillingDetailsEntry>(
                        sql,
                        new { RestaurantId = restaurantId });

                if (billingDetailsEntry == null)
                {
                    return NotFound("Billing account not found.");
                }

                if (authenticator.UserId != billingDetailsEntry.manager_id)
                {
                    return Unauthorised();
                }

                return Ok(billingDetailsEntry.ToDto());
            }
        }

        private record BillingDetailsEntry
        {
            public string id { get; init; }
            public Guid restaurant_id { get; init; }
            public bool billing_enabled { get; init; }
            public Guid manager_id { get; init; }

            public BillingDetails ToDto()
            {
                return new BillingDetails()
                {
                    Id = id,
                    RestaurantId = restaurant_id,
                    IsBillingEnabled = billing_enabled,
                };
            }
        }
    }
}
