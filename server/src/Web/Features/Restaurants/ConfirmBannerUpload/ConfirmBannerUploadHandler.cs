using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;

namespace Web.Features.Restaurants.ConfirmBannerUpload
{
    public class ConfirmBannerUploadHandler : IRequestHandler<ConfirmBannerUploadCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public ConfirmBannerUploadHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ConfirmBannerUploadCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetByManagerId(authenticator.UserId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            restaurant.Banner = command.Filename;

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
