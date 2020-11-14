using System.Collections.Generic;

namespace FoodSnap.Web.Envelopes
{
    public record ErrorEnvelope(string Message)
    {
        public Dictionary<string, string> Errors { get; init; }
    }
}
