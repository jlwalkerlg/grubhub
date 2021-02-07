using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Billing.GetOnboardingLink
{
    public class GetOnboardingLinkHandler : IRequestHandler<GetOnboardingLinkQuery, string>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBillingService billingService;

        public GetOnboardingLinkHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork, IBillingService billingService)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.billingService = billingService;
        }

        public async Task<Result<string>> Handle(
            GetOnboardingLinkQuery query,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants
                .GetById(new RestaurantId(query.RestaurantId));

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised();
            }

            var billingAccount = await unitOfWork.BillingAccounts
                .GetByRestaurantId(restaurant.Id);

            var link = await billingService.GenerateOnboardingLink(
                billingAccount.Id,
                restaurant.Id);

            return Result.Ok(link);
        }
    }
}
