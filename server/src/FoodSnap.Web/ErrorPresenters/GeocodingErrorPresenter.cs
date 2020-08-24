using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class GeocodingErrorPresenter : ErrorPresenter<GeocodingError>
    {
        protected override IActionResult PresentError(GeocodingError error)
        {
            var envelope = new ErrorEnvelope("Geo-coordinates could not be found for the given address.");

            var result = new ObjectResult(envelope);
            result.StatusCode = 400;

            return result;
        }
    }
}
