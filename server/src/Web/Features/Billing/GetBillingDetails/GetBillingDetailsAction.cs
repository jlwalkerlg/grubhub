using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Data.Models;
using Web.Domain.Users;
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

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurants/{restaurantId:guid}/billing")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
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
                    .QuerySingleOrDefaultAsync<BillingDetailsModel>(
                        sql,
                        new { RestaurantId = restaurantId });

                if (billingDetailsEntry == null)
                {
                    return StatusCode(200);
                }

                if (authenticator.UserId != billingDetailsEntry.manager_id)
                {
                    return Unauthorised();
                }

                return Ok(billingDetailsEntry.ToDto());
            }
        }
    }
}
