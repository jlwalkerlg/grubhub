using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Billing.SetupBilling
{
    public class SetupBillingHandler : IRequestHandler<SetupBillingCommand, string>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBillingService billingService;

        public SetupBillingHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork, IBillingService billingService)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.billingService = billingService;
        }

        public async Task<Result<string>> Handle(
            SetupBillingCommand command,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork
                .Restaurants
                .GetById(new RestaurantId(command.RestaurantId));

            if (restaurant.ManagerId != authenticator.UserId) return Error.Unauthorised();

            var billingAccount = restaurant.HasBillingAccount()
                ? await unitOfWork.BillingAccounts.GetById(restaurant.BillingAccountId)
                : null;

            if (billingAccount is null)
            {
                var billingAccountId = await billingService.CreateAccount(restaurant);

                billingAccount = new BillingAccount(new BillingAccountId(billingAccountId));
                restaurant.AddBillingAccount(billingAccount.Id);

                await unitOfWork.BillingAccounts.Add(billingAccount);
                await unitOfWork.Commit();
            }

            var link = await billingService.GenerateOnboardingLink(
                billingAccount.Id,
                restaurant.Id
            );

            return Result.Ok(link);
        }
    }
}
