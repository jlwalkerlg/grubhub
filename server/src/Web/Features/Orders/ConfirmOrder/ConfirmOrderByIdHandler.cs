using System.Threading;
using System.Threading.Tasks;
using Web.Features.Billing;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderByIdHandler : ConfirmOrderHandler, IRequestHandler<ConfirmOrderByIdCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public ConfirmOrderByIdHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IBillingService billingService,
            IAuthenticator authenticator) : base(unitOfWork, dateTimeProvider, billingService)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(ConfirmOrderByIdCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(command.Id);

            if (order is null) return Error.NotFound("Order not found.");

            if (order.UserId != authenticator.UserId) return Error.Unauthorised();

            return await Handle(order, cancellationToken);
        }
    }
}
