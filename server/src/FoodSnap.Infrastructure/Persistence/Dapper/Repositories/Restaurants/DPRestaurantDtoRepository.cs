using System;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application.Restaurants;

namespace FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Restaurants
{
    public class DPRestaurantDtoRepository : IRestaurantDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DPRestaurantDtoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RestaurantDto> GetById(Guid id)
        {
            var sql = @"
                SELECT
                    r.id,
                    r.manager_id,
                    r.name,
                    r.phone_number,
                    r.address,
                    r.latitude,
                    r.longitude,
                    r.status
                FROM
                    restaurants r
                WHERE
                    r.id = @Id";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var restaurant = await connection.QueryFirstOrDefaultAsync<RestaurantDto>(
                    sql,
                    new { Id = id });

                return restaurant;
            }
        }
    }
}
