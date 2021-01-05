using System.Collections.Generic;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public record UpdateCuisinesRequest
    {
        public List<string> Cuisines { get; init; } = new();
    }
}
