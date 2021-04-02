namespace Web.Features.Restaurants.ConfirmBannerUpload
{
    public class ConfirmBannerUploadCommand : IRequest
    {
        public string Filename { get; init; }
    }
}
