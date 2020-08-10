using System;
using System.Collections.Generic;

namespace FoodSnap.Application.Validation
{
    public class ValidationError : IError
    {
        public Dictionary<string, IValidationFailure> Errors { get; }

        public ValidationError(Dictionary<string, IValidationFailure> errors)
        {
            if (errors is null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            Errors = errors;
        }
    }
}
