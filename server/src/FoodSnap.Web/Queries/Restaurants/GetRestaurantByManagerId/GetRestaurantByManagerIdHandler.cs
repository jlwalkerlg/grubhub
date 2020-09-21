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

        public async Task<Result<RestaurantDto>> Handle(
            GetRestaurantByManagerIdQuery query,
            CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    restaurants.id,
                    restaurants.manager_id,
                    restaurants.name,
                    restaurants.phone_number,
                    restaurants.address_line1,
                    restaurants.address_line2,
                    restaurants.town,
                    restaurants.postcode,
                    restaurants.latitude,
                    restaurants.longitude,
                    restaurants.status
                FROM
                    restaurants
                WHERE
                    restaurants.manager_id = @Id";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<RestaurantDto>(
                    sql,
                    new { Id = query.ManagerId });

                return Result.Ok(user);
            }
        }
    }
}
