using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Restaurants;

namespace Application.Restaurants
{
    public interface ICuisineRepository
    {
        Task<List<Cuisine>> All();
    }
}
