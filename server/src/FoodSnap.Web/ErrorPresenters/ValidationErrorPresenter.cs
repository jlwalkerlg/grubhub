using System.Collections.Generic;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class ValidationErrorPresenter : ErrorPresenter<ValidationError>
    {
        protected override IActionResult PresentError(ValidationError error)
        {
            var errors = new Dictionary<string, string>();

            foreach (var item in error.Failures)
            {
                var key = item.Key;
                var message = GetErrorMessage(key, item.Value);

                errors.Add(key, message);
            }

            var envelope = new ValidationErrorEnvelope(
                "Invalid request.",
                errors);

            var result = new ObjectResult(envelope);
            result.StatusCode = 422;

            return result;
        }

        protected virtual string GetErrorMessage(string key, IValidationFailure failure)
        {
            if (failure is RequiredFailure)
            {
                return "Required.";
            }

            if (failure is EmailFailure)
            {
                return "Must be a valid email.";
            }

            if (failure is MinLengthFailure minLengthFailure)
            {
                return $"Must be at least {minLengthFailure.Length} characters in length.";
            }

            if (failure is PhoneNumberFailure)
            {
                return "Must be a valid phone number.";
            }

            if (failure is PostcodeFailure)
            {
                return "Must be a valid postcode.";
            }

            if (failure is EmailTakenFailure)
            {
                return "Must be a valid email.";
            }

            return "Invalid.";
        }
    }
}
