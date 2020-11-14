using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain;
namespace FoodSnap.Application.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsHandler : IRequestHandler<UpdateAuthUserDetailsCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public UpdateAuthUserDetailsHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAuthUserDetailsCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetById(authenticator.UserId);

            user.Name = command.Name;
            user.Email = new Email(command.Email);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
