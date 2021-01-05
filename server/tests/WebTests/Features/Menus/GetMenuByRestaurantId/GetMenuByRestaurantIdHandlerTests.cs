using System.Threading;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.GetMenuByRestaurantId;
using Xunit;
using static Web.Error;
using Web.Features.Menus;

namespace WebTests.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdHandlerTests
    {
        private readonly MenuDtoRepositoryFake menuDtoRepositoryFake;
        private readonly GetMenuByRestaurantIdHandler handler;

        public GetMenuByRestaurantIdHandlerTests()
        {
            menuDtoRepositoryFake = new MenuDtoRepositoryFake();

            handler = new GetMenuByRestaurantIdHandler(menuDtoRepositoryFake);
        }

        [Fact]
        public async Task It_Returns_The_Menu()
        {
            var menu = new MenuDto
            {
                RestaurantId = Guid.NewGuid(),
            };
            menuDtoRepositoryFake.Menus.Add(menu);

            var query = new GetMenuByRestaurantIdQuery { RestaurantId = menu.RestaurantId };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Same(menu, result.Value);
        }

        [Fact]
        public async Task It_Fails_If_Menu_Not_Found()
        {
            var query = new GetMenuByRestaurantIdQuery { RestaurantId = Guid.NewGuid() };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}
