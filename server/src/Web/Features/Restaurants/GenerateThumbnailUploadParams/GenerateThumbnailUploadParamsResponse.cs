using System.Collections.Generic;

namespace Web.Features.Restaurants.GenerateThumbnailUploadParams
{
    public record GenerateThumbnailUploadParamsResponse
    {
        public string Filename { get; init; }
        public string Url { get; init; }
        public IDictionary<string, string> Inputs { get; init; } = new Dictionary<string, string>();
    }
}
