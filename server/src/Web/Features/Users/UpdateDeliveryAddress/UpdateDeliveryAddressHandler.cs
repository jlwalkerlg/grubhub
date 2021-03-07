using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Services.Authentication;

namespace Web.Features.Users.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressHandler : IRequestHandler<UpdateDeliveryAddressCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateDeliveryAddressHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(UpdateDeliveryAddressCommand command, CancellationToken cancellationToken)
        {
            var customer = await unitOfWork.Users.GetCustomerById(authenticator.UserId);

            if (customer is null) return Error.NotFound("Customer not found.");

            customer.DeliveryAddress = new Address(
                command.AddressLine1,
                command.AddressLine2,
                command.City,
                new Postcode(command.Postcode));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
