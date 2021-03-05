using System;

namespace Web.Features.Menus.RemoveMenuCategory
{
    public record RemoveMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid CategoryId { get; init; }
    }
}
