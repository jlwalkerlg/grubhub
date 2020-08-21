using System;
using System.Collections.Generic;

namespace FoodSnap.Application.Validation
{
    public class ValidationError : IError
    {
        public Dictionary<string, IValidationFailure> Failures { get; }

        public ValidationError(Dictionary<string, IValidationFailure> failures)
        {
            if (failures is null)
            {
                throw new ArgumentNullException(nameof(failures));
            }

            Failures = failures;
        }
    }
}
