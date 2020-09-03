using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;

namespace FoodSnap.Application.Validation
{
    public class FluentValidator<TRequest> : AbstractValidator<TRequest>, IValidator<TRequest>
        where TRequest : IRequest
    {
        protected IRuleBuilderInitial<TRequest, TProperty> CascadeRuleFor<TProperty>(Expression<Func<TRequest, TProperty>> expression)
        {
            return RuleFor(expression).Cascade(CascadeMode.Stop);
        }

        public async new Task<Result> Validate(TRequest request)
        {
            var result = await base.ValidateAsync(request);

            if (result.IsValid)
            {
                return Result.Ok();
            }

            var failures = new Dictionary<string, IValidationFailure>();

            foreach (var error in result.Errors)
            {
                failures.Add(error.PropertyName, (IValidationFailure)error.CustomState);
            }

            return Result.Fail(new ValidationError(failures));
        }
    }
}
