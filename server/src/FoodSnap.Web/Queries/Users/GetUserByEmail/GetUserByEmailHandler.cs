using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;

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
                    id,
                    name,
                    email,
                    password,
                    role
                FROM users
                WHERE email = @Email";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<UserDto>(
                    sql,
                    new { Email = "walker.jlg@gmail.com" });

                return Result.Ok(user);
            }
        }
    }
}
