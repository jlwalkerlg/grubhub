using FluentValidation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Services.Validation
{
    public class RequestValidator<TRequest> : AbstractValidator<TRequest>, IValidator<TRequest>
    {
        protected IRuleBuilderInitial<TRequest, TProperty> CascadeRuleFor<TProperty>(Expression<Func<TRequest, TProperty>> expression)
        {
            return RuleFor(expression).Cascade(CascadeMode.Stop);
        }

        public new async Task<Result> Validate(TRequest request)
        {
            var result = await ValidateAsync(request);

            if (result.IsValid) return Result.Ok();

            var errors = result.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);

            return Error.ValidationError(errors);
        }
    }
}
