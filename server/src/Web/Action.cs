using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Envelopes;
using static Web.Error;

namespace Web
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private readonly Dictionary<ErrorType, int> codes = new()
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

            if (!codes.TryGetValue(error.Type, out var code))
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
