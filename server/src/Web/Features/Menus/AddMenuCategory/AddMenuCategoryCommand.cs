using System;

namespace Web.Features.Menus.AddMenuCategory
{
    public record AddMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string Name { get; init; }
    }
}
