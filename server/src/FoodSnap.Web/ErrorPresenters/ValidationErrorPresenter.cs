using System.Collections.Generic;
using FoodSnap.Application.Validation;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class ValidationErrorPresenter : ErrorPresenter<ValidationError>
    {
        protected override IActionResult PresentError(ValidationError error)
        {
            var errors = new Dictionary<string, string>();

            foreach (var item in error.Errors)
            {
                errors.Add(item.Key, item.Value.ToString());
            }

            var envelope = new ValidationErrorEnvelope(
                "Invalid request.",
                errors);

            var result = new ObjectResult(envelope);
            result.StatusCode = 422;

            return result;
        }
    }
}
