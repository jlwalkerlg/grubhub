using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;
using static FoodSnap.Application.Error;

namespace FoodSnap.Web.Actions
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private Dictionary<ErrorType, int> codes = new()
        {
            { ErrorType.BadRequest, 400 },
            { ErrorType.Unauthenticated, 401 },
            { ErrorType.Unauthorised, 403 },
            { ErrorType.NotFound, 404 },
            { ErrorType.ValidationError, 422 },
            { ErrorType.Internal, 500 },
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
