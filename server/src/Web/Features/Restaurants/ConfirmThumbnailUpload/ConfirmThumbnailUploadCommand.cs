namespace Web.Features.Restaurants.ConfirmThumbnailUpload
{
    public class ConfirmThumbnailUploadCommand : IRequest
    {
        public string Filename { get; init; }
    }
}
