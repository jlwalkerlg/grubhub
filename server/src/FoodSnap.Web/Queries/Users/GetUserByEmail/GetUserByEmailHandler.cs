using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;
using FoodSnap.Web.Actions.Users;

namespace FoodSnap.Web.Queries.Users.GetUserByEmail
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetUserByEmailHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<UserDto>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    ""Id"",
                    ""Name"",
                    ""Email"",
                    ""Password"",
                    ""UserType"" as ""Role""
                FROM ""Users""
                WHERE ""Email"" = @Email";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Email = "walker.jlg@gmail.com" });

                return Result.Ok(user);
            }
        }
    }
}
