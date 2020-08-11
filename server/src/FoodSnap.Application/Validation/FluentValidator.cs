using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;

namespace FoodSnap.Application.Validation
{
    public class FluentValidator<TRequest> : AbstractValidator<TRequest>, IValidator<TRequest>
        where TRequest : IRequest
    {
        public FluentValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }

        public async new Task<Result> Validate(TRequest request)
        {
            var result = await base.ValidateAsync(request);

            if (result.IsValid)
            {
                return Result.Ok();
            }

            var errors = new Dictionary<string, IValidationFailure>();

            foreach (var failure in result.Errors)
            {
                errors.Add(failure.PropertyName, (IValidationFailure)failure.CustomState);
            }

            return Result.Fail(new ValidationError(errors));
        }
    }
}
