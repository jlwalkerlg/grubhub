using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Restaurants;

namespace Web.Features.Restaurants
{
    public interface ICuisineRepository
    {
        Task<List<Cuisine>> All();
    }
}
