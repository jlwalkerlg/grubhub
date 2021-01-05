using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants;

namespace Web.Data.Dapper.Repositories.Restaurants
{
    public class DPCuisineDtoRepository : ICuisineDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DPCuisineDtoRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<CuisineDto>> All()
        {
            var sql = "SELECT c.name FROM cuisines c ORDER BY c.name ASC";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                return (await connection.QueryAsync<CuisineDto>(sql)).ToList();
            }
        }
    }
}
