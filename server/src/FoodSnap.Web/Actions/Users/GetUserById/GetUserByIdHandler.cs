using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Shared;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Actions.Users.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetUserByIdHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    u.id,
                    u.name,
                    u.email,
                    u.password,
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
                var user = await connection.QueryFirstOrDefaultAsync<UserDto>(
                    sql,
                    new { Id = query.Id });

                return Result.Ok(user);
            }
        }
    }
}
