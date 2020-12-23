using System;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Restaurants;
using Application.Restaurants.SearchRestaurants;
using Application.Services.Geocoding;
using ApplicationTests.Services.Geocoding;
using Domain;
using Xunit;
using static Application.Error;

namespace ApplicationTests.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsHandlerTests
    {
        private readonly GeocoderSpy geocoder;
        private readonly RestaurantDtoRepositoryFake restaurantDtoRepositoryFake;
        private readonly SearchRestaurantsHandler handler;

        public SearchRestaurantsHandlerTests()
        {
            geocoder = new GeocoderSpy();

            restaurantDtoRepositoryFake = new RestaurantDtoRepositoryFake();

            handler = new SearchRestaurantsHandler(geocoder, restaurantDtoRepositoryFake);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("fkrjengk")]
        public async Task It_Fails_If_Postcode_Is_Invalid(string postcode)
        {
            var query = new SearchRestaurantsQuery()
            {
                Postcode = postcode,
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public async Task It_Fails_If_Geocoding_Fails()
        {
            geocoder.Result = Error.Internal("Invalid postal code.");

            var query = new SearchRestaurantsQuery()
            {
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.BadRequest, result.Error.Type);
        }

        [Fact]
        public async Task It_Returns_The_Restaurants()
        {
            geocoder.Result = Result.Ok(new GeocodingResult()
            {
                FormattedAddress = "",
                Coordinates = new Coordinates(53, -2),
            });

            restaurantDtoRepositoryFake.Restaurants.Add(new RestaurantDto()
            {
                Id = Guid.NewGuid(),
            });

            var query = new SearchRestaurantsQuery()
            {
                Postcode = "MN12 1NM"
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(53, restaurantDtoRepositoryFake.SearchCoordinates.Latitude);
            Assert.Equal(-2, restaurantDtoRepositoryFake.SearchCoordinates.Longitude);
            Assert.Single(result.Value);
            Assert.Same(restaurantDtoRepositoryFake.Restaurants[0], result.Value[0]);
        }
    }
}
