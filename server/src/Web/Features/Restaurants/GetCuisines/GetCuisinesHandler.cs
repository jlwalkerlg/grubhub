using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Features.Restaurants.GetCuisines
{
    public class GetCuisinesHandler : IRequestHandler<GetCuisinesQuery, List<CuisineDto>>
    {
        private readonly ICuisineDtoRepository repository;

        public GetCuisinesHandler(ICuisineDtoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<List<CuisineDto>>> Handle(
            GetCuisinesQuery query,
            CancellationToken cancellationToken)
        {
            return Result.Ok(await repository.All());
        }
    }
}
