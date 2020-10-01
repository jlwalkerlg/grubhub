using System.Collections.Generic;
using FoodSnap.Application;
using FoodSnap.Web.Actions;
using FoodSnap.Web.Envelopes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions
{
    public class ActionTests
    {
        private readonly AlwaysFailsAction action;

        public ActionTests()
        {
            action = new AlwaysFailsAction();
        }

        [Fact]
        public void It_Returns_500_For_Internal_Errors()
        {
            var error = Error.Internal("Database failed.");

            var result = action.Execute(error) as ObjectResult;
            var envelope = result.Value as ErrorEnvelope;

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(error.Message, envelope.Message);
        }

        [Fact]
        public void It_Returns_400_For_Bad_Request_Errors()
        {
            var error = Error.BadRequest("Malformed request.");

            var result = action.Execute(error) as ObjectResult;
            var envelope = result.Value as ErrorEnvelope;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal(error.Message, envelope.Message);
        }

        [Fact]
        public void It_Returns_422_For_Validation_Errors()
        {
            var errors = new Dictionary<string, string>
            {
                { "Name", "Required." },
            };
            var error = Error.ValidationError(errors);

            var result = action.Execute(error) as ObjectResult;
            var envelope = result.Value as ErrorEnvelope;

            Assert.Equal(422, result.StatusCode);
            Assert.Equal(error.Message, envelope.Message);
            Assert.Equal(errors, envelope.Errors);
        }

        private class AlwaysFailsAction : Action
        {
            public IActionResult Execute<T>(T error) where T : Error
            {
                return PresentError(error);
            }
        }
    }
}
