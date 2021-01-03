using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public interface ICuisineDtoRepository
    {
        Task<List<CuisineDto>> All();
    }
}
