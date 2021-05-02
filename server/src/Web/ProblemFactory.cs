using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web
{
    public class ProblemFactory
    {
        private static readonly Dictionary<ErrorType, int> Statuses = new()
        {
            { ErrorType.BadRequest, 400 },
            { ErrorType.Unauthenticated, 401 },
            { ErrorType.Unauthorised, 403 },
            { ErrorType.NotFound, 404 },
            { ErrorType.ValidationError, 422 },
            { ErrorType.Internal, 500 },
        };

        public ProblemDetails Make(Error error)
        {
            if (error.Type == ErrorType.ValidationError)
            {
                return new ValidationProblemDetails(
                    error.Errors.ToDictionary(
                        x => x.Key.ToCamelCase(),
                        x => new[] {x.Value}));
            }

            if (!Statuses.TryGetValue(error.Type, out var status))
            {
                status = 500;
            }

            return new ProblemDetails()
            {
                Status = status,
                Detail = error.Message,
            };
        }
    }
}
