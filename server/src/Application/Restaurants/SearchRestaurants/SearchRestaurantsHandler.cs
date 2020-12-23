using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Geocoding;

namespace Application.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsHandler : IRequestHandler<SearchRestaurantsQuery, List<RestaurantDto>>
    {
        private static Regex regex = new Regex(
            @"[A-Z]{2}\d{1,2} ?\d[A-Z]{2}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));

        private readonly IGeocoder geocoder;
        private readonly IRestaurantDtoRepository repository;

        public SearchRestaurantsHandler(IGeocoder geocoder, IRestaurantDtoRepository repository)
        {
            this.geocoder = geocoder;
            this.repository = repository;
        }

        public async Task<Result<List<RestaurantDto>>> Handle(SearchRestaurantsQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Postcode) || !regex.IsMatch(query.Postcode))
            {
                return Error.BadRequest("Invalid postcode.");
            }

            var geocodingResult = await geocoder.Geocode(query.Postcode);

            if (!geocodingResult.IsSuccess)
            {
                return Error.BadRequest("Sorry, we don't recognise that postcode.");
            }

            var restaurants = await repository.Search(geocodingResult.Value.Coordinates);

            return Result.Ok(restaurants);
        }
    }
}
