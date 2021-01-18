using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.UpdateMenuItem
{
    [Authenticate]
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
