using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Application.Validation;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class ErrorPresenter : IErrorPresenter
    {
        public IActionResult Present(IError error)
        {
            if (error is ValidationError validationError)
            {
                return PresentValidationError(validationError);
            }

            return new StatusCodeResult(500);
        }

        private IActionResult PresentValidationError(ValidationError error)
        {
            var errors = new Dictionary<string, string>();

            foreach (var item in error.Errors)
            {
                errors.Add(item.Key, item.Value.ToString());
            }

            var result = new ObjectResult(errors);
            result.StatusCode = 422;

            return result;
        }
    }
}
