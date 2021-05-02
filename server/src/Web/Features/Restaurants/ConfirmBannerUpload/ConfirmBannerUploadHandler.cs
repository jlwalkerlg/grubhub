using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Restaurants.ConfirmBannerUpload
{
    public class ConfirmBannerUploadHandler : IRequestHandler<ConfirmBannerUploadCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;

        public ConfirmBannerUploadHandler(
            IAuthenticator authenticator,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(ConfirmBannerUploadCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetByManagerId(authenticator.UserId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            restaurant.Banner = command.Filename;

            await unitOfWork.Outbox.Add(new RestaurantUpdatedEvent(restaurant.Id, dateTimeProvider.UtcNow));
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
