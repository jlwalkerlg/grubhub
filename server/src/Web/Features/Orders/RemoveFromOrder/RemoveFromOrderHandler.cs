using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;

namespace Web.Features.Orders.RemoveFromOrder
{
    public class RemoveFromOrderHandler : IRequestHandler<RemoveFromOrderCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public RemoveFromOrderHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveFromOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetActiveOrderForUser(authenticator.UserId);

            if (order == null)
            {
                return Error.NotFound("Active order not found.");
            }

            var result = order.RemoveItem(command.MenuItemId);

            if (!result)
            {
                return result;
            }

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
