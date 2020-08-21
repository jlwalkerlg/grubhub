using FoodSnap.Application;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class DefaultErrorPresenter : ErrorPresenter<Error>
    {
        protected override IActionResult PresentError(Error error)
        {
            var envelope = new ErrorEnvelope("Unknown error.");

            var result = new ObjectResult(envelope);
            result.StatusCode = 500;

            return result;
        }
    }
}
