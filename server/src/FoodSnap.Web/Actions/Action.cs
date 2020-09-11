using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private Dictionary<Error.ErrorType, int> codes
             = new Dictionary<Error.ErrorType, int>
             {
                 { Error.ErrorType.BadRequest, 400 },
                 { Error.ErrorType.ValidationError, 422 },
                 { Error.ErrorType.ServerError, 500 },
             };

        protected IActionResult PresentError(Error error)
        {
            var envelope = new ErrorEnvelope
            {
                Message = error.Message,
                Errors = error.Errors,
            };

            return new ObjectResult(envelope)
            {
                StatusCode = codes[error.Type],
            };
        }
    }
}
