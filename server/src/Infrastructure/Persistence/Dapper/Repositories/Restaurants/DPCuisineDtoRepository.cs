using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Application.Restaurants;
using Application.Services;

namespace Infrastructure.Persistence.Dapper.Repositories.Restaurants
{
    public class DPCuisineDtoRepository : ICuisineDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IClock clock;

        public DPCuisineDtoRepository(
            IDbConnectionFactory dbConnectionFactory, IClock clock)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.clock = clock;
        }

        public async Task<List<CuisineDto>> All()
        {
            var sql = "SELECT c.name FROM cuisines c";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                return (await connection.QueryAsync<CuisineDto>(sql)).ToList();
            }
        }
    }
}
