using System;
using System.Collections.Generic;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public record UpdateCuisinesCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public List<string> Cuisines { get; init; }
    }
}
