using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Shared;

namespace FoodSnap.Application.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdHandler : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
    {
        private readonly IRestaurantDtoRepository repository;

        public GetRestaurantByIdHandler(IRestaurantDtoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<RestaurantDto>> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
        {
            var restaurant = await repository.GetById(query.Id);

            if (restaurant == null)
            {
                return Result<RestaurantDto>.Fail(Error.NotFound("Restaurant not found."));
            }

            return Result.Ok(restaurant);
        }
    }
}
