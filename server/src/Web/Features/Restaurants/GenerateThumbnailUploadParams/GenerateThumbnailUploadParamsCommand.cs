namespace Web.Features.Restaurants.GenerateThumbnailUploadParams
{
    public record GenerateThumbnailUploadParamsCommand : IRequest<GenerateThumbnailUploadParamsResponse>
    {
        public string Filename { get; init; }
    }
}
