using System;
using Application.Services.Authentication;

namespace Application.Menus.RemoveMenuItem
{
    [Authenticate]
    public record RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
        public string ItemName { get; init; }
    }
}
