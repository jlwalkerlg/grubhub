using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Features.Restaurants.SearchRestaurants;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

namespace WebTests.Features.Restaurants.SearchRestaurants
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
            geocoder.Result = Error.Internal("Geocoding failed.");

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
