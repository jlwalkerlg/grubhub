using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Services.Authentication;

namespace Web.Features.Users.UpdateAccountDetails
{
    public class UpdateAccountDetailsHandler : IRequestHandler<UpdateAccountDetailsCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateAccountDetailsHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(UpdateAccountDetailsCommand command, CancellationToken cancellationToken)
        {
            var customer = await unitOfWork.Users.GetCustomerById(authenticator.UserId);

            if (customer is null) return Error.NotFound("Customer not found.");

            customer.Name = command.Name;
            customer.MobileNumber = new MobileNumber(command.MobileNumber);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
