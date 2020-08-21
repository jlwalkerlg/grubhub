using System.Collections.Generic;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class ValidationErrorPresenterTests
    {
        private readonly ValidationErrorPresenter presenter;

        public ValidationErrorPresenterTests()
        {
            presenter = new ValidationErrorPresenter();
        }

        [Fact]
        public void It_Returns_A_422_Response()
        {
            var failures = new Dictionary<string, IValidationFailure>
            {
                { "Name", new RequiredFailure() },
            };

            var error = new ValidationError(failures);
            var result = presenter.Present(error) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(422, result.StatusCode);

            var errors = result.Value as Dictionary<string, string>;

            Assert.NotNull(errors);
            Assert.NotEmpty(errors["Name"]);
        }
    }
}
