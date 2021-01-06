using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Validation
{
    public class ValidationMiddleware<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Result, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationMiddleware(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var validationResult = await validators
                .First()
                .Validate(request);

            if (!validationResult)
            {
                var result = new TResponse();
                result.Error = validationResult.Error;
                return result;
            }

            return await next();
        }
    }
}
