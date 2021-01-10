using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Web.Services.Geocoding;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsHandler : IRequestHandler<SearchRestaurantsQuery, List<RestaurantSearchResult>>
    {
        private static readonly Regex regex = new Regex(
            @"[A-Z]{2}\d{1,2} ?\d[A-Z]{2}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));

        private readonly IGeocoder geocoder;
        private readonly IRestaurantSearcher searcher;

        public SearchRestaurantsHandler(IGeocoder geocoder, IRestaurantSearcher searcher)
        {
            this.geocoder = geocoder;
            this.searcher = searcher;
        }

        public async Task<Result<List<RestaurantSearchResult>>> Handle(SearchRestaurantsQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Postcode) || !regex.IsMatch(query.Postcode))
            {
                return Error.BadRequest("Invalid postcode.");
            }

            var geocodingResult = await geocoder.Geocode(query.Postcode);

            if (!geocodingResult)
            {
                return Error.BadRequest("Sorry, we don't recognise that postcode.");
            }

            var restaurants = await searcher.Search(
                geocodingResult.Value.Coordinates,
                query.Options);

            return Result.Ok(restaurants);
        }
    }
}
