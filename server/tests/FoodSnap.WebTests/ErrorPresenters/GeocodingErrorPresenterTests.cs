using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class GeocodingErrorPresenterTests
    {
        private readonly GeocodingErrorPresenter presenter;

        public GeocodingErrorPresenterTests()
        {
            presenter = new GeocodingErrorPresenter();
        }

        [Fact]
        public void It_Returns_A_400_Response()
        {
            var error = new GeocodingError();

            var result = presenter.Present(error) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var envelope = result.Value as ErrorEnvelope;

            Assert.NotNull(envelope.Message);
        }
    }
}
