using System;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Data;
using Web.Services.Authentication;

namespace Web.Features.Users.GetAuthUser
{
    public class GetAuthUserAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetAuthUserAction(IAuthenticator authenticator, IDbConnectionFactory dbConnectionFactory)
        {
            this.authenticator = authenticator;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [Authorize]
        [HttpGet("/auth/user")]
        public async Task<IActionResult> Execute()
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var user = await connection
                .QuerySingleOrDefaultAsync<UserModel>(
                    @"SELECT
                        u.id,
                        u.first_name,
                        u.last_name,
                        u.email,
                        u.role,
                        u.mobile_number,
                        u.address_line1,
                        u.address_line2,
                        u.city,
                        u.postcode,
                        r.id as restaurant_id,
                        r.name as restaurant_name
                    FROM
                        users u
                    LEFT JOIN restaurants r ON r.manager_id = u.id
                    WHERE
                        u.Id = @Id",
                    new { Id = authenticator.UserId.Value });

            return user is null
                ? NotFound("User not found.")
                : Ok(user);
        }

        public class UserModel
        {
            public Guid Id { get; init; }
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public string Email { get; init; }
            public string Role { get; init; }
            public string MobileNumber { get; init; }
            public string AddressLine1 { get; init; }
            public string AddressLine2 { get; init; }
            public string City { get; init; }
            public string Postcode { get; init; }
            public Guid? RestaurantId { get; init; }
            public string RestaurantName { get; init; }
        }
    }
}
