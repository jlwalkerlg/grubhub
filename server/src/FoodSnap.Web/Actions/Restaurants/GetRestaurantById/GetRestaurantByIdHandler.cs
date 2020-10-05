using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Shared;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdHandler : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetRestaurantByIdHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<RestaurantDto>> Handle(
            GetRestaurantByIdQuery query,
            CancellationToken cancellationToken)
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
                    new { Id = query.Id });

                return Result.Ok(restaurant);
            }
        }
    }
}
