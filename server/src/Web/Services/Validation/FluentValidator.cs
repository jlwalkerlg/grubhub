using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Services.Validation
{
    public class FluentValidator<TRequest> : AbstractValidator<TRequest>, IValidator<TRequest>
    {
        protected IRuleBuilderInitial<TRequest, TProperty> CascadeRuleFor<TProperty>(Expression<Func<TRequest, TProperty>> expression)
        {
            return RuleFor(expression).Cascade(CascadeMode.Stop);
        }

        public new async Task<Result> Validate(TRequest request)
        {
            var result = await base.ValidateAsync(request);

            if (result.IsValid)
            {
                return Result.Ok();
            }

            var errors = new Dictionary<string, string>();

            foreach (var error in result.Errors)
            {
                errors.Add(error.PropertyName, error.ErrorMessage);
            }

            return Error.ValidationError(errors);
        }
    }
}
