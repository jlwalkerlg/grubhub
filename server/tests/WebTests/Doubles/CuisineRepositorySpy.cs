using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Features.Restaurants;

namespace WebTests.Doubles
{
    public class CuisineRepositorySpy : ICuisineRepository
    {
        public List<Cuisine> Cuisines = new();

        public Task<List<Cuisine>> All()
        {
            return Task.FromResult(Cuisines);
        }

        public Task Add(Cuisine cuisine)
        {
            Cuisines.Add(cuisine);
            return Task.CompletedTask;
        }
    }
}
