using System.Threading;
using System.Threading.Tasks;
using Web.Features.Billing;
using Web.Services.DateTimeServices;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderByPaymentIntentIdHandler : ConfirmOrderHandler, IRequestHandler<ConfirmOrderByPaymentIntentIdCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public ConfirmOrderByPaymentIntentIdHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IBillingService billingService) : base(unitOfWork, dateTimeProvider, billingService)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmOrderByPaymentIntentIdCommand command,
            CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetByPaymentIntentId(command.PaymentIntentId);

            return await Handle(order, cancellationToken);
        }
    }
}
