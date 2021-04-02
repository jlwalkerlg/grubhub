namespace Web.Features.Restaurants.GenerateBannerUploadParams
{
    public record GenerateBannerUploadParamsCommand : IRequest<GenerateBannerUploadParamsResponse>
    {
        public string Filename { get; init; }
    }
}
