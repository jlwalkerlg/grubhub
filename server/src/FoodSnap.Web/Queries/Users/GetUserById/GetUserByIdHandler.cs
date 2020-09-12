using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Queries.GetUserById
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
                    id,
                    name,
                    email,
                    password,
                    role
                FROM users
                WHERE id = @Id";

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
