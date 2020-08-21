using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Application.Validation;
using FoodSnap.Web.ErrorPresenters;
using Xunit;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class ErrorPresenterFactoryTests
    {
        private readonly ErrorPresenterFactory factory;

        public ErrorPresenterFactoryTests()
        {
            factory = new ErrorPresenterFactory();
        }

        [Fact]
        public void It_Returns_A_Default_Error_Presenter()
        {
            var error = new Error();

            Assert.IsType<DefaultErrorPresenter>(factory.Make(error));
        }

        [Fact]
        public void It_Returns_A_Validation_Error_Presenter()
        {
            var failures = new Dictionary<string, IValidationFailure>();
            var error = new ValidationError(failures);

            Assert.IsType<ValidationErrorPresenter>(factory.Make(error));
        }
    }
}
