using System;

namespace Web.Features.Menus.RemoveMenuItem
{
    public record RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid ItemId { get; init; }
    }
}
