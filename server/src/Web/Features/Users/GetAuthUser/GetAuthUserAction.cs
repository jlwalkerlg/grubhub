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
            var sql = @"
                SELECT
                    u.id,
                    u.name,
                    u.email,
                    u.role,
                    r.id as restaurant_id,
                    r.name as restaurant_name
                FROM
                    users u
                LEFT JOIN restaurants r ON r.manager_id = u.id
                WHERE
                    u.Id = @Id";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var user = await connection
                    .QuerySingleOrDefaultAsync<UserDto>(
                        sql,
                        new { Id = authenticator.UserId.Value });

                return user is null
                    ? NotFound("User not found.")
                    : Ok(user);
            }
        }
    }
}
