using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;

namespace Web.Features.Restaurants.ConfirmThumbnailUpload
{
    public class ConfirmThumbnailUploadHandler : IRequestHandler<ConfirmThumbnailUploadCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public ConfirmThumbnailUploadHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ConfirmThumbnailUploadCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetByManagerId(authenticator.UserId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            restaurant.Thumbnail = command.Filename;

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
