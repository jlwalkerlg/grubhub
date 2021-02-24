using Web.Features.Cuisines;

namespace Web.Data.Models
{
    public record RestaurantCuisineModel
    {
        public string cuisine_name { get; init; }

        public CuisineDto ToCuisineDto()
        {
            return new CuisineDto()
            {
                Name = cuisine_name,
            };
        }
    }
}
