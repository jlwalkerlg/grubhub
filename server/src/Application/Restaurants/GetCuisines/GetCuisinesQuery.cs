using System.Collections.Generic;

namespace Application.Restaurants.GetCuisines
{
    public record GetCuisinesQuery : IRequest<List<CuisineDto>>;
}
