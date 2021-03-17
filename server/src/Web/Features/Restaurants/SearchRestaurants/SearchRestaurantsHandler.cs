using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Services.Geocoding;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsHandler : IRequestHandler<SearchRestaurantsQuery, SearchRestaurantsResponse>
    {
        private readonly IGeocoder geocoder;
        private readonly IRestaurantSearcher searcher;

        public SearchRestaurantsHandler(IGeocoder geocoder, IRestaurantSearcher searcher)
        {
            this.geocoder = geocoder;
            this.searcher = searcher;
        }

        public async Task<Result<SearchRestaurantsResponse>> Handle(SearchRestaurantsQuery query, CancellationToken cancellationToken)
        {
            if (!Postcode.IsValid(query.Postcode))
            {
                return Error.BadRequest("Invalid postcode.");
            }

            var (coordinates, error) = await geocoder.LookupCoordinates(query.Postcode);

            if (error) return Error.BadRequest("Sorry, we don't recognise that postcode.");

            var response = await searcher.Search(
                coordinates,
                query.Options);

            return Result.Ok(response);
        }
    }
}
