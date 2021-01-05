using System.Collections.Generic;

namespace Web.Features.Restaurants.GetCuisines
{
    public record GetCuisinesQuery : IRequest<List<CuisineDto>>;
}
