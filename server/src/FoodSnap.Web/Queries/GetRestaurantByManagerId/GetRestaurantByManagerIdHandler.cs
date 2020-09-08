using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;

namespace FoodSnap.Web.Queries.GetRestaurantByManagerId
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
                    ""Id"",
                    ""ManagerId"",
                    ""Name"",
                    ""PhoneNumber"",
                    ""AddressLine1"",
                    ""AddressLine2"",
                    ""Town"",
                    ""Postcode"",
                    ""Latitude"",
                    ""Longitude"",
                    ""Status""
                FROM
                    ""Restaurants""
                WHERE
                    ""ManagerId"" = @ManagerId;";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var restaurant = await connection.QueryFirstOrDefaultAsync<RestaurantDto>(
                    sql, new { ManagerId = query.ManagerId });

                return Result.Ok(restaurant);
            }
        }
    }
}
