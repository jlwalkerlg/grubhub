using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Restaurants.ConfirmThumbnailUpload
{
    public class ConfirmThumbnailUploadHandler : IRequestHandler<ConfirmThumbnailUploadCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;

        public ConfirmThumbnailUploadHandler(
            IAuthenticator authenticator,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(ConfirmThumbnailUploadCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetByManagerId(authenticator.UserId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            restaurant.Thumbnail = command.Filename;

            await unitOfWork.Publish(new RestaurantUpdatedEvent(restaurant.Id, dateTimeProvider.UtcNow));
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
