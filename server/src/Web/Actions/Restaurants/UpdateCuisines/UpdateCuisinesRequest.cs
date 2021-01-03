using System.Collections.Generic;

namespace Web.Actions.Restaurants.UpdateCuisines
{
    public record UpdateCuisinesRequest
    {
        public List<string> Cuisines { get; init; } = new();
    }
}
