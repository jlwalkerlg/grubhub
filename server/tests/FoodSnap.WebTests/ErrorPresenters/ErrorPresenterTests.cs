using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class ErrorPresenterTests
    {
        private readonly ErrorPresenter presenter;

        public ErrorPresenterTests()
        {
            presenter = new ErrorPresenter();
        }

        [Fact]
        public void It_Returns_500_For_Unknown_Errors()
        {
            var error = new Error();
            var result = presenter.Present(error) as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void It_Returns_422_For_Validation_Errors()
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
