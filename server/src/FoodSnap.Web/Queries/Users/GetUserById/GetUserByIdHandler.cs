using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Queries.Users.GetUserById
{
    public class GetAuthDataHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetAuthDataHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    users.id,
                    users.name,
                    users.email,
                    users.password,
                    users.role
                FROM
                    users
                WHERE
                    users.Id = @Id";
            ;

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<UserDto>(
                    sql,
                    new { Id = query.Id });

                return Result.Ok(user);
            }
        }
    }
}
