using System;
using System.Collections.Generic;
using Web.Services.Authentication;

namespace Web.Features.Restaurants.UpdateCuisines
{
    [Authenticate]
    public record UpdateCuisinesCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public List<string> Cuisines { get; init; }
    }
}
