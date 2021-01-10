using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Cuisines;

namespace Web.Features.Cuisines
{
    public interface ICuisineRepository
    {
        Task<List<Cuisine>> All();
    }
}
