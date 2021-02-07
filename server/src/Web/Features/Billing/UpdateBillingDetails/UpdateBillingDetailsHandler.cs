using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Billing;

namespace Web.Features.Billing.UpdateBillingDetails
{
    public class UpdateBillingDetailsHandler : IRequestHandler<UpdateBillingDetailsCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateBillingDetailsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateBillingDetailsCommand command,
            CancellationToken cancellationToken)
        {
            var account = await unitOfWork.BillingAccounts
                .GetById(new BillingAccountId(command.BillingAccountId));

            if (account == null)
            {
                return Error.NotFound("Billing account not found.");
            }

            if (command.IsBillingEnabled)
            {
                account.Enable();
            }
            else
            {
                account.Disable();
            }

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
