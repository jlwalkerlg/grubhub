using System.Collections.Generic;
using FoodSnap.Shared;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private Dictionary<Error.ErrorType, int> codes = new()
        {
            { Shared.Error.ErrorType.BadRequest, 400 },
            { Shared.Error.ErrorType.Unauthenticated, 401 },
            { Shared.Error.ErrorType.Unauthorised, 403 },
            { Shared.Error.ErrorType.NotFound, 404 },
            { Shared.Error.ErrorType.ValidationError, 422 },
            { Shared.Error.ErrorType.Internal, 500 },
        };

        protected IActionResult Error(Error error)
        {
            var envelope = new ErrorEnvelope(error.Message)
            {
                Errors = error.Errors,
            };

            int code;
            if (!codes.TryGetValue(error.Type, out code))
            {
                code = 500;
            }

            return new ObjectResult(envelope)
            {
                StatusCode = code,
            };
        }

        protected OkObjectResult Ok<T>(T data)
        {
            return base.Ok(new DataEnvelope<T>(data));
        }
    }
}
