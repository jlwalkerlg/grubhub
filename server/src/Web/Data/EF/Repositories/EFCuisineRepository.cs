using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Restaurants;
using Web.Domain.Restaurants;
using Microsoft.EntityFrameworkCore;

namespace Web.Data.EF.Repositories
{
    public class EFCuisineRepository : ICuisineRepository
    {
        private readonly AppDbContext context;

        public EFCuisineRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task<List<Cuisine>> All()
        {
            return context.Cuisines.ToListAsync();
        }
    }
}
