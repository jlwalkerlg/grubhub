using System;

namespace FoodSnap.Web.Envelopes
{
    public class ErrorEnvelope
    {
        public string Message { get; }

        public ErrorEnvelope(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }
    }
}
