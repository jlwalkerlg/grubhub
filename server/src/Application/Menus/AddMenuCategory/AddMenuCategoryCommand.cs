using System;
using Application.Services.Authentication;

namespace Application.Menus.AddMenuCategory
{
    [Authenticate]
    public record AddMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string Name { get; init; }
    }
}
