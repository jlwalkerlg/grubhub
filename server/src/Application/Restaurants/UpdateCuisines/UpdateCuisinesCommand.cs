using System;
using System.Collections.Generic;
using Application.Services.Authentication;

namespace Application.Restaurants.UpdateCuisines
{
    [Authenticate]
    public record UpdateCuisinesCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public List<string> Cuisines { get; init; }
    }
}
