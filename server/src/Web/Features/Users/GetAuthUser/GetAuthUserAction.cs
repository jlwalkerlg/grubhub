using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        [HttpGet("/auth/user")]
        public async Task<IActionResult> Execute()
        {
            if (!authenticator.IsAuthenticated)
            {
                return Error(Web.Error.Unauthenticated());
            }

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

                return user == null
                    ? Error(Web.Error.NotFound("User not found."))
                    : Ok(user);
            }
        }
    }
}
