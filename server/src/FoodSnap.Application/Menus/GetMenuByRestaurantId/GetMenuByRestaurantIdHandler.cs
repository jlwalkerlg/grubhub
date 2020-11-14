using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application;

namespace FoodSnap.Application.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdHandler : IRequestHandler<GetMenuByRestaurantIdQuery, MenuDto>
    {
        private readonly IMenuDtoRepository repository;

        public GetMenuByRestaurantIdHandler(IMenuDtoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<MenuDto>> Handle(GetMenuByRestaurantIdQuery request, CancellationToken cancellationToken)
        {
            var menu = await repository.GetByRestaurantId(request.RestaurantId);

            if (menu == null)
            {
                return Result<MenuDto>.Fail(Error.NotFound("Menu not found."));
            }

            return Result.Ok(menu);
        }
    }
}
