using Shouldly;
using System.Threading.Tasks;
using Web;
using Web.Features.Restaurants.SearchRestaurants;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsHandlerTests
    {
        private readonly GeocoderSpy geocoder;
        private readonly RestaurantSearcherFake restaurantSearcherFake;
        private readonly SearchRestaurantsHandler handler;

        public SearchRestaurantsHandlerTests()
        {
            geocoder = new GeocoderSpy();

            restaurantSearcherFake = new RestaurantSearcherFake();

            handler = new SearchRestaurantsHandler(geocoder, restaurantSearcherFake);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("fkrjengk")]
        public async Task It_Fails_If_The_Postcode_Is_Invalid(string postcode)
        {
            var query = new SearchRestaurantsQuery()
            {
                Postcode = postcode,
            };

            var result = await handler.Handle(query, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_Geocoding_Fails()
        {
            geocoder.LookupCoordinatesResult = Error.Internal("Geocoding failed.");

            var query = new SearchRestaurantsQuery()
            {
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(query, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
