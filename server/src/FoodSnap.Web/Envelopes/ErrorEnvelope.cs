using System.Collections.Generic;

namespace FoodSnap.Web.Envelopes
{
    public class ErrorEnvelope
    {
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
