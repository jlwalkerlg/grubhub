using System.Collections.Generic;

namespace Web.Envelopes
{
    public record ErrorEnvelope(string Message)
    {
        public Dictionary<string, string> Errors { get; init; }
    }
}
