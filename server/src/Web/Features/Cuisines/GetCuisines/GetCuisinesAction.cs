using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Features.Cuisines.GetCuisines
{
    public class GetCuisinesAction : Action
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetCuisinesAction(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        [HttpGet("/cuisines")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Execute()
        {
            using var connection = await dbConnectionFactory.OpenConnection();

            var cuisines = await connection.QueryAsync<CuisineModel>(
                    "SELECT c.name FROM cuisines c ORDER BY c.name ASC");

            return Ok(cuisines);
        }

        public class CuisineModel
        {
            public string Name { get; init; }
        }
    }
}
