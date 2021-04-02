using System.Collections.Generic;

namespace Web.Services.Storage
{
    public record ImageUploadParams
    {
        public string Url { get; init; }
        public IDictionary<string, string> Inputs { get; init; } = new Dictionary<string, string>();
    }
}
