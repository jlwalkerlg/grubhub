using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web;
using Web.Services.Validation;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Services.Validation
{
    public class ValidationMiddlewareTests
    {
        private readonly DummyCommandValidatorSpy validatorSpy;
        private readonly List<IValidator<DummyCommand>> validators;
        private readonly ValidationMiddleware<DummyCommand, Result> middleware;

        public ValidationMiddlewareTests()
        {
            validatorSpy = new DummyCommandValidatorSpy();

            validators = new List<IValidator<DummyCommand>>
            {
                validatorSpy
            };

            middleware = new ValidationMiddleware<DummyCommand, Result>(validators);
        }

        [Fact]
        public async Task It_Returns_A_Validation_Error_If_Validation_Fails()
        {
            var error = Error.ValidationError(new Dictionary<string, string>());
            validatorSpy.Result = error;

            RequestHandlerDelegate<Result> next = () => Task.FromResult(Result.Ok());

            var result = await middleware.Handle(
                new DummyCommand(),
                default,
                next);

            Assert.False(result);
            Assert.Same(error, result.Error);
        }

        [Fact]
        public async Task It_Returns_The_Handler_Result_If_Validation_Passes()
        {
            validatorSpy.Result = Result.Ok();

            var handlerResult = Result.Ok();
            RequestHandlerDelegate<Result> next = () => Task.FromResult(handlerResult);

            var result = await middleware.Handle(
                new DummyCommand(),
                default,
                next);

            Assert.True(result);
            Assert.Same(handlerResult, result);
        }
    }
}
