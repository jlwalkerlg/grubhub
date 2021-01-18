using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.RemoveMenuItem
{
    [Authenticate]
    public record RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid ItemId { get; init; }
    }
}
