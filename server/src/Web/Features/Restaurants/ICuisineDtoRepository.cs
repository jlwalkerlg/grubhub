using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Features.Restaurants
{
    public interface ICuisineDtoRepository
    {
        Task<List<CuisineDto>> All();
    }
}
