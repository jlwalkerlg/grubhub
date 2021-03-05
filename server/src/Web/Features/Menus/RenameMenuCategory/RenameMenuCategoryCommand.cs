using System;

namespace Web.Features.Menus.RenameMenuCategory
{
    public record RenameMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid CategoryId { get; init; }
        public string NewName { get; init; }
    }
}
