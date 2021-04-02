using System.Collections.Generic;

namespace Web.Features.Restaurants.GenerateBannerUploadParams
{
    public record GenerateBannerUploadParamsResponse
    {
        public string Filename { get; init; }
        public string Url { get; init; }
        public IDictionary<string, string> Inputs { get; init; } = new Dictionary<string, string>();
    }
}
