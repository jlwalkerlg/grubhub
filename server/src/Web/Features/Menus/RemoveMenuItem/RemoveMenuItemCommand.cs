using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.RemoveMenuItem
{
    [Authenticate]
    public record RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
        public string ItemName { get; init; }
    }
}
