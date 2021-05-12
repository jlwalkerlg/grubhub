using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
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
            using var connection = await dbConnectionFactory.OpenConnection();

            var billingDetails = await connection.QuerySingleOrDefaultAsync<BillingDetailsModel>(
                @"SELECT
                    ba.id,
                    ba.billing_enabled AS is_billing_enabled,
                    r.manager_id
                FROM
                    billing_accounts ba
                    INNER JOIN restaurants r ON r.billing_account_id = ba.id
                WHERE
                    r.id = @RestaurantId",
                new { RestaurantId = restaurantId });

            if (billingDetails is null)
            {
                return StatusCode(200);
            }

            if (authenticator.UserId != billingDetails.ManagerId)
            {
                return Unauthorised();
            }

            return Ok(billingDetails);
        }

        public class BillingDetailsModel
        {
            public string Id { get; init; }
            public bool IsBillingEnabled { get; init; }
            [JsonIgnore] public Guid ManagerId { get; init; }
        }
    }
}
