using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.AddMenuCategory
{
    [Authenticate]
    public record AddMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string Name { get; init; }
    }
}
