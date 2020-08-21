using System;
using System.Collections.Generic;

namespace FoodSnap.Web.Envelopes
{
    public class ValidationErrorEnvelope : ErrorEnvelope
    {
        public Dictionary<string, string> Errors { get; }

        public ValidationErrorEnvelope(string message, Dictionary<string, string> errors) : base(message)
        {
            if (errors is null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            Errors = errors;
        }
    }
}
