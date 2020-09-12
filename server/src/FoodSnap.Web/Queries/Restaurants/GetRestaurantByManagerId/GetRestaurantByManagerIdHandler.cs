using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Queries.Restaurants.GetRestaurantByManagerId
{
    public class GetRestaurantByManagerIdHandler : IRequestHandler<GetRestaurantByManagerIdQuery, RestaurantDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetRestaurantByManagerIdHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<RestaurantDto>> Handle(GetRestaurantByManagerIdQuery query, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    id,
                    manager_id,
                    name,
                    phone_number,
                    address_line1,
                    address_line2,
                    town,
                    postcode,
                    latitude,
                    longitude,
                    status
                FROM
                    restaurants
                WHERE
                    manager_id = @ManagerId";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var restaurant = await connection.QueryFirstOrDefaultAsync<RestaurantDto>(
                    sql, new { ManagerId = query.ManagerId });

                return Result.Ok(restaurant);
            }
        }
    }
}
