using System;
using Application.Services.Authentication;

namespace Application.Menus.RemoveMenuCategory
{
    [Authenticate]
    public record RemoveMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
    }
}
