using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Services.Storage;

namespace Web.Features.Restaurants.GenerateBannerUploadParams
{
    public class GenerateBannerUploadParamsHandler : IRequestHandler<GenerateBannerUploadParamsCommand, GenerateBannerUploadParamsResponse>
    {
        private static readonly Dictionary<string, string> ContentTypes = new()
        {
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"png", "image/png"},
            {"gif", "image/gif"},
        };

        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageStore imageStore;

        public GenerateBannerUploadParamsHandler(
            IAuthenticator authenticator,
            IUnitOfWork unitOfWork,
            IImageStore imageStore)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.imageStore = imageStore;
        }

        public async Task<Result<GenerateBannerUploadParamsResponse>> Handle(
            GenerateBannerUploadParamsCommand command,
            CancellationToken cancellationToken)
        {
            var ext = command.Filename?.Split('.').Last() ?? "";

            if (!ContentTypes.ContainsKey(ext)) return Error.BadRequest("File extension not accepted.");

            var restaurant = await unitOfWork.Restaurants.GetByManagerId(authenticator.UserId);

            if (restaurant is null) return Error.BadRequest("Restaurant not found.");

            var contentType = ContentTypes[ext];

            var filename = $"banner_{Guid.NewGuid()}.{ext}";

            var uploadParams = await imageStore.GenerateUploadParams(
                $"restaurants/{restaurant.Id.Value}/{filename}",
                contentType,
                minSize: 1,
                maxSize: 2 * (int)Math.Pow(2, 20));

            return Result.Ok(new GenerateBannerUploadParamsResponse()
            {
                Filename = filename,
                Url = uploadParams.Url,
                Inputs = uploadParams.Inputs,
            });
        }
    }
}
