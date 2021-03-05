using System;

namespace Web.Features.Menus.UpdateMenuItem
{
    public record UpdateMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid ItemId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
