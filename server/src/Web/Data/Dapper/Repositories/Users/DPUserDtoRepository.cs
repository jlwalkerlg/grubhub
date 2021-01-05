using Dapper;
using System;
using System.Threading.Tasks;
using Web.Features.Users;

namespace Web.Data.Dapper.Repositories.Users
{
    public class DPUserDtoRepository : IUserDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DPUserDtoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<UserDto> GetById(Guid id)
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
                return await connection
                    .QuerySingleOrDefaultAsync<UserDto>(
                        sql,
                        new { Id = id });
            }
        }
    }
}
