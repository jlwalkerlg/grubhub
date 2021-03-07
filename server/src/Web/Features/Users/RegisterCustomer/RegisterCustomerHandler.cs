using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Users;
using Web.Services.Authentication;
using Web.Services.Hashing;

namespace Web.Features.Users.RegisterCustomer
{
    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHasher hasher;
        private readonly IAuthenticator authenticator;

        public RegisterCustomerHandler(IUnitOfWork unitOfWork, IHasher hasher, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.hasher = hasher;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
            if (await unitOfWork.Users.EmailExists(command.Email))
            {
                return Error.ValidationError(nameof(command.Email), "Email already taken.");
            }

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                command.Name,
                new Email(command.Email),
                hasher.Hash(command.Password));

            await unitOfWork.Users.Add(customer);

            await unitOfWork.Commit();

            await authenticator.SignIn(customer);

            return Result.Ok();
        }
    }
}
