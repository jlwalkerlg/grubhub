using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
namespace FoodSnap.Application.Users.GetAuthUser
{
    public class GetAuthUserHandler : IRequestHandler<GetAuthUserQuery, UserDto>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUserDtoRepository repository;

        public GetAuthUserHandler(IAuthenticator authenticator, IUserDtoRepository repository)
        {
            this.authenticator = authenticator;
            this.repository = repository;
        }

        public async Task<Result<UserDto>> Handle(GetAuthUserQuery query, CancellationToken cancellationToken)
        {
            var user = await repository.GetById(authenticator.UserId.Value);

            if (user == null)
            {
                return Result<UserDto>.Fail(Error.NotFound("User not found."));
            }

            return Result.Ok(user);
        }
    }
}
