using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.RemoveMenuCategory
{
    [Authenticate]
    public record RemoveMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
    }
}
